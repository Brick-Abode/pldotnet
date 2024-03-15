#!/bin/bash

rm -rf tests/npgsql/sql/
mkdir tests/npgsql/sql/
chmod 0777 tests/npgsql/sql/

files=(npgsql/test/Npgsql.Tests/*.cs)
files+=(npgsql/test/Npgsql.Tests/Types/*.cs)

if [ ${#files[@]} -gt 0 ]; then
  dotnet run --project tests/npgsql/ "${files[@]}"
else
  echo "No .cs files found in the directories npgsql/test/Npgsql.Tests/ and npgsql/test/Npgsql.Tests/Types/"
fi

rm -rf tests/npgsql/bin tests/npgsql/obj

# Loop over each line in the file
while read -r line; do
  # Concatenate ".sql" to the end of the line
  file="tests/npgsql/sql/${line}.sql"
  echo "$file"

  # Run the command and redirect the output to a file with ".out" extension
  output_file="${file%.sql}.out" # Replace .sql extension with .out
  sudo -u postgres psql < "$file" 2>&1 | tee "$output_file" || /etc/init.d/postgresql restart

  # Check if the output file contains "Working fine on pldotnet"
  if ! grep -q "Working fine on pldotnet 👍" "$output_file"; then
    echo "Error: $output_file does not contain 'Working fine on pldotnet 👍' so the test failed."
    exit 1
  fi
done < "tests/npgsql/results/working_tests.txt"
