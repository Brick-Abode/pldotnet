import pandas as pd
import re
import sys
import os
import psycopg2
from typing import List

directory_to_save = "tests/npgsql/results/"


def match_regex(data: str, regex: str) -> str:
    result_regex = re.compile(regex)
    result_match = re.search(result_regex, data)

    if result_match:
        result = result_match.group(1)
        return result.strip()
    else:
        raise Exception("There is no result for the regex expression.")


def create_data_frame_from_postgresql(table_name: str):
    conn = psycopg2.connect(
        host="localhost",
        database="postgres",
        user="postgres",
        password="postgres",
    )
    df = pd.read_sql(f"SELECT * FROM {table_name}", conn)
    return df


def create_data_frame_from_csv(path: str):
    df = pd.read_csv(path)
    return df


def find_cause_of_failure(result: str, test: str) -> str:
    # Test is working
    if "Working fine on pldotnet 👍" in result:
        return "Working fine on pldotnet 👍", "Working fine 👍"

    # PostgresException
    if "ErrorData in C#. Here's the message: " in result:
        message = match_regex(result, "ErrorData in C#. Here's the message: +(.*)")
        return message, "Failed in C"


    # Test fails with an ERROR message
    if "ERROR:  " in result:
        message = match_regex(result, "ERROR: +(.*)")
        return message, "Failed in C"

    # Test fails in the PG_TRY statement of a C function in pldotnet_spi.c
    # PG_TRY();
    # {
    # }
    # PG_CATCH();
    # {
    #     elog(WARNING, "Exception: pldotnet_X");
    #     PG_RE_THROW();
    # }
    if "WARNING:  Exception: " in result:
        fail_on = match_regex(result, "WARNING:  Exception: +(.*)")
        return (
            f'the "{fail_on}" function failed with no error message',
            "Failed in C",
        )

    # Test fails and breaks the database without an explanation
    if "server closed the connection unexpectedly" in result:
        return (
            "server closed the connection unexpectedly.",
            "Failed in C",
        )

    # Test fails in C#, we could return the caught exception
    if "Fail on C#..." in result:
        lines = result.split("\n")
        start = 0
        end = len(lines)
        for cont, line in zip(range(end), lines):
            if "Fail on C#..." in line:
                start = cont + 1

            if (
                test in line
            ):  # todo - also check the file name with the function name
                end = cont
                break
        return (
            " \\n ".join(
                [
                    line[: len(line) - 1].strip()
                    for line in lines[start:end]
                    if "--->" not in line
                    # if ("at PlDotNET." in line or "at Npgsql." in line)
                ]
            ),
            "Failed in C#",
        )

    raise Exception(f"Failed to extract the cause of the failure to {test}.")


def export_data_frame(df, file_name: str):
    # Write the markdown table to a file
    with open(directory_to_save + file_name, "w") as f:
        f.write(df.to_markdown(index=False))


def compare_working_tests(current_working_tests: List[str]):
    # Reading file
    with open(
        directory_to_save + "working_tests.txt", "r", encoding="utf-8"
    ) as f:
        last_working_tests = f.read().split("\n")

    not_working_anymore = []

    for test in last_working_tests:
        if test not in current_working_tests:
            not_working_anymore.append(test)

    working_now = []

    for test in current_working_tests:
        if test not in last_working_tests:
            working_now.append(test)

    if working_now:
        aux = "- " + "\n- ".join(working_now)
        print(
            f"Great! You make {len(working_now)} test(s) pass. Here is the list:\n{aux}"
        )

    if not_working_anymore:
        aux = "- " + "\n- ".join(not_working_anymore)
        print(
            f"Please, take a look at your changes. You broke {len(not_working_anymore)} test(s)."
            f"\nThe following tests are not working anymore:\n{aux}"
        )

    if not working_now and not not_working_anymore:
        print("No changes to the tests that were already passing.")
    else:

        last_working_tests.extend(working_now)
        with open(
            directory_to_save + "working_tests.txt", "w", encoding="utf-8"
        ) as f:
            f.write("\n".join(last_working_tests))


def create_data_frame_from_out_files():
    # TODO - set it dynamically
    directory = "tests/npgsql/sql"

    # Create pandas dataframe
    df = pd.DataFrame(columns=["Feature", "Test Name", "Result", "Failed in"])

    # List of tests that are working
    working_tests = []

    # Loop over all the .out files in the directory
    for filename in sorted(os.listdir(directory)):
        if filename.endswith(".out"):
            # Read the contents of the file and set the variable value
            with open(os.path.join(directory, filename), "r") as f:
                try:
                    file = f.read()
                    split_name = filename.replace(".out", "").split("_")
                    feature = split_name[0]
                    test_name = "_".join(split_name[1:])
                    result, failed_on = find_cause_of_failure(
                        file, f"{feature}.{test_name}"
                    )

                    result = result.replace("System.AggregateException: One or more errors occurred. ", '')

                    result = (
                        f"{failed_on}: {result}"
                        if result != "Working fine on pldotnet 👍"
                        else result
                    )

                    if result == "Working fine on pldotnet 👍":
                        working_tests.append(filename.replace(".out", ""))

                    row = pd.Series(
                        [feature, test_name, result, failed_on],
                        index=["Feature", "Test Name", "Result", "Failed in"],
                    )

                    df = pd.concat([df, row.to_frame().T], ignore_index=True)

                except Exception as excepetion_message:
                    print(excepetion_message)

    compare_working_tests(working_tests)

    # Grouping by the failure
    grouped_df_result = df.groupby(["Result"]).size().reset_index(name="Count")
    grouped_df_result = grouped_df_result[["Count", "Result"]]
    grouped_df_result = grouped_df_result.sort_values("Count", ascending=False)
    export_data_frame(grouped_df_result, "results.md")

    # Add column with failed tests
    grouped_df_result["Failed Tests"] = grouped_df_result.apply(
        lambda x: ", ".join(df.loc[df["Result"] == x["Result"], "Test Name"])
        if x["Result"] != "Working fine on pldotnet 👍"
        else "OK",
        axis=1,
    )
    export_data_frame(grouped_df_result, "results_with_test_names.md")

    # Grouping by the location it failed
    grouped_df_failed = (
        df.groupby(["Failed in"]).size().reset_index(name="Count")
    )
    grouped_df_failed = grouped_df_failed[["Count", "Failed in"]]
    grouped_df_failed = grouped_df_failed.sort_values("Count", ascending=False)
    print(
        f"\nHere is a summary of the results:\n{grouped_df_failed.to_markdown(index=False)}")
    export_data_frame(grouped_df_failed, "fail_types.md")


def main(argv) -> None:
    """Main function"""
    create_data_frame_from_out_files()


if __name__ == "__main__":
    main(sys.argv)
