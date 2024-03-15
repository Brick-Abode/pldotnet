#!/bin/bash

rm -rf tests/npgsql/sql/
mkdir tests/npgsql/sql/
chmod 0777 tests/npgsql/sql/

# dotnet run --project tests/npgsql/ dotnet_src/npgsql/test/Npgsql.Tests/Types/NumericTypeTests.cs

files=(dotnet_src/npgsql/test/Npgsql.Tests/*.cs)
files+=(dotnet_src/npgsql/test/Npgsql.Tests/Types/*.cs)

if [ ${#files[@]} -gt 0 ]; then
  dotnet run --project tests/npgsql/ "${files[@]}"
else
  echo "No .cs files found in the directories dotnet_src/npgsql/test/Npgsql.Tests/ and dotnet_src/npgsql/test/Npgsql.Tests/Types/"
fi

rm -rf tests/npgsql/bin tests/npgsql/obj

echo 'DROP TABLE IF EXISTS NPGSQL_TESTS;CREATE TABLE NPGSQL_TESTS(FEATURE TEXT, TEST_NAME TEXT, RESULT TEXT, RESULT_BOOL BOOLEAN);' | (sudo -u postgres  psql)

could_not_compile="Here are the compilation results"
add_in_the_table="INSERT 0 1"
for file in tests/npgsql/sql/*.sql
do
    echo $file
    output_file="${file//.sql/.out}"
    cat $file | (sudo -u postgres psql 2>&1 || /etc/init.d/postgresql restart) | tee $output_file
    # if ! grep -q "$could_not_compile" $output_file && ! grep -q "$add_in_the_table" $output_file; then
    #     content=$(cat $output_file)
    #     all_line=$(cat $file | grep "SELECT '")
    #     feature=$(echo $all_line | awk -F, '{print $1}' | awk -F\' '{print $2}')
    #     test_name=$(echo $all_line | awk -F, '{print $2}' | awk -F\' '{print $2}')
    #     echo "INSERT INTO NPGSQL_TESTS (FEATURE, TEST_NAME, RESULT) SELECT '$feature', '$test_name', '$content';" | (sudo -u postgres  psql)
    # fi
done

# cat tests/npgsql/npgsql.sql | (sudo -u postgres  psql 2>&1) | tee npgsql_tests_results.out