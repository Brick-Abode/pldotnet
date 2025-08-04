import os
import re
from typing import List, Dict, Tuple

import wordninja


# Define the source directory containing SQL files
source_directory: str = ""

destination_directory: str = ""


def to_pascal_case(s):
    words = wordninja.split(s)
    return "".join(word.capitalize() for word in words)


def extract_function_definitions(file_content: str) -> List[str]:
    # Regex to extract function definitions from the provided content
    pattern = r"CREATE OR REPLACE FUNCTION.+?$$ LANGUAGE plcsharp;"
    functions = re.findall(pattern, file_content, re.DOTALL)
    return functions


def extract_details(test_case: str) -> Tuple[str, str, str, str]:
    # Pattern modification to extract the full input SQL and the condition from the SELECT
    pattern = r"(WITH.+?)INSERT INTO automated_test_results \(FEATURE, TEST_NAME, RESULT\)\nSELECT '(.+?)', '(.+?)', (.+?);"

    match = re.search(pattern, test_case, re.DOTALL)
    if match:
        input_sql = match.group(1).strip()
        feature = match.group(2)
        test_name = match.group(3)
        # Assuming the last part of the SELECT condition is the expected value
        condition = match.group(4).strip()

        return feature, test_name, input_sql, condition

    return None


def generate_csharp_class(
    class_name: str,
    function_name: str,
    csharp_function: str,
    return_type: str,
    arguments: List[Dict[str, str]],
    test_cases: List[str],
    is_strict: bool,
) -> str:

    print(f"Arguments: {arguments}")
    test_cases_str_list = []

    # In C# the string break if its a single " , need to replace with ""
    if csharp_function:
        csharp_function = csharp_function.replace('"', '""')

    for case in test_cases:
        details = extract_details(case)
        if details:
            feature, test_case, input_sql, condition = details
            # Formatting changes for multiline strings and correct escaping
            formatted_str = f"""
        yield return new object[]
        {{
            "{feature}",
            "{test_case}",
            @"{input_sql}",
            "{condition}"
        }};"""
            test_cases_str_list.append(formatted_str)

    test_cases_method_content = "\n        ".join(test_cases_str_list)

    if not test_cases_method_content:
        test_cases_method_content = "// No test cases"

    arguments_str = ", ".join(
        f'new FunctionArgument("{arg["Name"]}", "{arg["Type"]}")' for arg in arguments
    )
    print(f"Arguments parsed: {arguments_str}")

    # The template
    template = f"""
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class Base{class_name} : PlDotNetTest
{{

    protected abstract string FunctionBody {{ get; }}
    protected abstract LanguageType Language {{ get; }}

    protected string cteStatement = string.Empty;

    public Base{class_name}()
    {{
        FunctionInfo = new SqlFunctionInfo
        {{
            Name = "{function_name}",
            Arguments = new List<FunctionArgument> {{ {arguments_str} }},
            ReturnType = "{return_type}",
            Body = FunctionBody,
            Language = Language,
            IsStrict = {str(is_strict).lower()},
        }};
    }}

     protected void SetupTest(string cteStatement)
    {{
        this.cteStatement = cteStatement;
        FunctionInfo.CteStatement = cteStatement;
    }}

    public static IEnumerable<object[]> TestCases()
    {{
{test_cases_method_content}
    }}


    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test{class_name}(
        string featureName,
        string testName,
        string cteStatement,
        string customAssertion,
        string querySuffix = null
    )
    {{
        SetupTest(cteStatement);
        RunTestWithSuffix(featureName, testName, this.cteStatement, customAssertion, querySuffix);
    }}
}}

[Trait("Language", "CSharp")]
[Trait("Category", "Record")]
public class {class_name}Csharp : Base{class_name}
{{
        protected override string FunctionBody => @"
{csharp_function}
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}}
"""
    return template


def get_sql_files(directory: str) -> List[str]:
    sql_files: List[str] = []
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith(".sql"):
                sql_files.append(os.path.join(root, file))
    return sql_files


def clean_file_name(file_name: str) -> str:
    cuted_file_name = file_name[4:-4].title()
    pascal_file_name = to_pascal_case(cuted_file_name)
    return pascal_file_name


def create_csharp_test_file(data: dict, folder_name: str) -> None:
    print(">>> Creating: ", data["function_name"])

    print(str(data))

    function_name = data["function_name"]
    function_body = data["function_body"]
    test_cases = data["test_cases"]
    is_strict = data.get("is_strict", False)
    cs_content = generate_csharp_class(
        class_name=f"{function_name}Tests",
        function_name=function_name,
        csharp_function=function_body,
        arguments=data["arguments"],
        test_cases=test_cases,
        is_strict=is_strict,
        return_type=data["return_type"],
    )
    cs_file_name = f"{function_name}Tests.cs"

    cs_folder_path = os.path.join(destination_directory, folder_name)
    cs_file_path = os.path.join(cs_folder_path, cs_file_name)

    os.makedirs(cs_folder_path, exist_ok=True)

    with open(cs_file_path, "w") as cs_file:
        cs_file.write(cs_content)


def split_on_create_function(sql_content: str) -> list:
    segments = re.split(r"(?=CREATE OR REPLACE FUNCTION)", sql_content)
    if not segments[0].strip() or segments[0].strip().startswith("--"):
        segments = segments[1:]
    return segments


def extract_function_and_tests(segment: str) -> Dict[str, object]:
    # Extracting the function definition
    function_match = re.search(
        r"CREATE OR REPLACE FUNCTION\s+([\w_]+)\s*\((.*?)\)\s*RETURNS\s+(.*?)\s+AS",
        segment,
        re.DOTALL,
    )
    is_strict = False
    function_name = to_pascal_case(function_match.group(1)) if function_match else None
    function_body = (
        re.search(r"\$\$(.+?)\$\$", segment, re.DOTALL).group(1).strip()
        if function_match
        else None
    )

    raw_args = function_match.group(2).split(",") if function_match else []
    arguments = []
    for arg in raw_args:
        parts = arg.strip().rsplit(" ", 1)
        if len(parts) == 2:
            arg_name, arg_type = parts
            arguments.append(
                {
                    "Name": arg_name.strip(),
                    "Type": (
                        arg_type.strip() + ")"
                        if "(" in arg_type and ")" not in arg_type
                        else arg_type.strip()
                    ),
                }
            )

    return_type = function_match.group(3).strip() if function_match else ""

    # Capturing full test cases, including CTEs
    test_cases = re.findall(r"(WITH.+?;)", segment, re.DOTALL)

    return {
        "function_name": function_name,
        "function_body": function_body,
        "arguments": arguments,
        "return_type": return_type,
        "test_cases": test_cases,
        "is_strict": is_strict,  # Ensure 'is_strict' is always returned
    }


sql_files: List[str] = get_sql_files(source_directory)

for sql_file in sql_files:
    with open(sql_file, "r") as sql_file_content:
        sql_content = sql_file_content.read()

    functions_and_tests_list = split_on_create_function(sql_content)

    file_name_only = os.path.basename(sql_file)
    folder_name = clean_file_name(file_name_only)

    print(
        f"SQL File: {sql_file} - Number of Functions: {len(functions_and_tests_list)}"
    )

    for function_declaration in functions_and_tests_list:
        parsed_data_obj = extract_function_and_tests(function_declaration)
        print(
            f"Function: {parsed_data_obj['function_name']} - Number of Test Cases: {len(parsed_data_obj['test_cases'])}"
        )

        if len(parsed_data_obj["test_cases"]) > 0:
            create_csharp_test_file(parsed_data_obj, folder_name)
        else:
            print(
                f"Skipping {parsed_data_obj['function_name']} due to no associated test cases."
            )

print("Conversion completed.")
