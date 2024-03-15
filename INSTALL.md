# 1. Installation Guide

This guide provides quick instructions for installing pldotnet
and its dependencies. You can find more detailed instructions on
how to install pldotnet and run verification tests in the [GitHub
wiki](https://github.com/Brick-Abode/pldotnet/wiki/).

## 1.1. Requirements

Before installing pldotnet, make sure that you have the following software
installed on your system:

- [PostgreSQL](https://www.postgresql.org/) 10 or greater
  - Refer to the [PostgreSQL download
  page](https://www.postgresql.org/download/) for instructions.
- [.NET](https://learn.microsoft.com/en-us/dotnet/) 6.0 or greater
  - Refer to [Microsoft installation
  page](https://learn.microsoft.com/en-us/dotnet/core/install/) for
  instructions.

*pldotnet* also requires `libglib2.0` and `make`. You can install them
using the following command:

```bash
sudo apt install -y libglib2.0 make
```

## 1.2. Installing *pldotnet*

1- Download the Debian package for pldotnet from the Brick Abode website,
selecting the package that corresponds to your version of PostgreSQL.

2- Install the package using the following command:

```bash
sudo dpkg -i postgres-*-pldotnet_0.9-1_amd64.deb
```

## 1.3. Building Debian packages

To build Debian packages for pldotnet using Docker, you need to have
`docker` and `docker-compose` installed on your system. Refer to
the [Docker documentation]((https://docs.docker.com/desktop/)) for
installation instructions.

Before building the packages, specify the version of PostgreSQL you are
using in the `pldotnet/debian/pgversions` file.

To build the pldotnet packages, follow these steps:

```bash
# Clone the pldotnet repository:
git clone https://github.com/Brick-Abode/pldotnet.git

# Change into the repository directory:
cd pldotnet

# Initialize submodules
git submodule update --init --recursive

# Create a docker container to build the packages
docker-compose up pldotnet-build
```

The packages will be stored in the `pldotnet/debian/packages` directory.

If you want to try out pldotnet before installing it on your system,
you can use a Docker container by running the following commands:

```bash
# Create a docker container and open the bash terminal
docker-compose run --rm pldotnet-build bash

# Install pldotnet inside the container
dpkg -i debian/packages/postgres-15-pldotnet_0.9-1_amd64.deb
```

### 1.3.1. Debian packages for ARM processors

To create pldotnet packages for installation on ARM processors,
specify the version of PostgreSQL you are using in the
`pldotnet/debian/pgversions-arm` file. Then, run the following command
from the pldotnet directory:

```bash
# Create a docker container to build the ARM packages
docker-compose up pldotnet-build-arm
```

After building the packages, they will be stored in the
`pldotnet/debian/packages` directory. If you want to install them in a
clean container, you can use the following instructions:

```bash
# Create a docker container and open the bash terminal
docker-compose run --rm pldotnet-build-arm bash

# Install pldotnet inside the container
dpkg -i debian/packages/postgresql-15-pldotnet_0.9-1_arm64.deb
```

## 1.4. Installing the built packages

To install the pldotnet packages that you have built, use the `dpkg -i`
command and specify the path to the package file:

```bash
dpkg -i path/to/package.deb
```

## 1.5. Conducting pldotnet Tests

pldotnet includes comprehensive tests to verify the functionality of all
implemented features and supported data types. These tests are categorized
into two formats: xUnit tests utilizing the Npgsql library, and direct SQL
tests. Explore the xUnit tests within the `tests/xUnit` directory and the
SQL tests in the `tests/csharp` and `tests/fsharp` directories, corresponding
to C# and F# languages, respectively.

For convenience, pldotnet provides `make` targets to facilitate the execution
of these tests:

### 1.5.1. xUnit Tests

#### 1.5.1.1. Understanding tests results

The xUnit testing framework is designed to provide comprehensive feedback on
the pldotnet tests, pinpointing the exact step where a failure occurs.
Understanding these results is crucial for debugging and enhancing the
reliability of the pldotnet. Here's how to interpret the outcomes:

There are three possible failure scenarios for a pldotnet tests:

1.Function Creation Failure: This occurs when there are compilation errors
during the function creation process.
2.Function Execution Failure: This happens when an error is thrown during
the execution of C# or F# functions.
3.Assertion Error: This is identified when a function's actual return value
does not match the expected outcome.

After executing the tests, the terminal will display the results, indicating
the success or failure of the tests.

- All tests pass: a green message confirms that all tests have been successful.
  ```bash
  Passed!  - Failed:     0, Passed:   881, Skipped:     0, Total:   881, Duration: 27 s
  ```

- Some tests fail: a red message indicates the number of failed tests.

  ```bash
  Failed!  - Failed:     15, Passed:   866, Skipped:     0, Total:   881, Duration: 26 s
  ```

In case of a failure, the terminal provides detailed information about the nature and location of the issue:

- Compiling generated code: `Failed to create function in .NET`

  ```bash
  Failed Sum2IntegerTests.TestSum2Integer(featureName: "c#-int4", testName: "sum2Integer1",
    input: "32770, 100", expectedResult: "= INTEGER '32870'") [51 ms]
  Error Message:
   Failed to create function in .NET
  Expected: True
  Actual:   False
    Stack Trace:
      at PlDotNetTest.RunGenericTest(String featureName, String testName, String input,
        String expectedResult) in /app/pldotnet/tests/xUnit/PlDotNetTest.cs:line 348
      at Sum2IntegerTests.TestSum2Integer(String featureName, String testName, String input,
        String expectedResult) in /app/pldotnet/tests/xUnit/tests/csharp/Integers/Sum2IntegerTests.cs:line 48
  ```

- Executing function: `Failed to execute the function`

  ```bash
  Failed Sum2IntegerTests.TestSum2Integer(featureName: "c#-int4", testName: "sum2Integer1",
    input: "32770, 100", expectedResult: "= INTEGER '32870'") [52 ms]
  Error Message:
   Failed to execute the function.
  Expected: True
  Actual:   False
    Stack Trace:
      at PlDotNetTest.RunGenericTest(String featureName, String testName, String input,
        String expectedResult) in /app/pldotnet/tests/xUnit/PlDotNetTest.cs:line 349
      at Sum2IntegerTests.TestSum2Integer(String featureName, String testName, String input,
        String expectedResult) in /app/pldotnet/tests/xUnit/tests/csharp/Integers/Sum2IntegerTests.cs:line 48
  ```

- Returned unexpected value: `Test did not return the expected value`

  ```bash
  Failed Sum2IntegerTests.TestSum2Integer(featureName: "c#-int4", testName: "sum2Integer1",
  input: "32770, 100", expectedResult: "= INTEGER '3287'") [36 ms]
  Error Message:
   Test did not return the expected value.
  Expected: True
  Actual:   False
    Stack Trace:
      at PlDotNetTest.RunGenericTest(String featureName, String testName, String input,
      String expectedResult) in /app/pldotnet/tests/xUnit/PlDotNetTest.cs:line 352
      at Sum2IntegerTests.TestSum2Integer(String featureName, String testName, String input,
      String expectedResult) in /app/pldotnet/tests/xUnit/tests/csharp/Integers/Sum2IntegerTests.cs:line 48
  ```

#### 1.5.1.2. Running the tests

Run the xUnit tests to ensure the pldotnet's functionality with C# and F# is
intact. The commands are as follows:

- For both C# and F# tests: This command runs all xUnit tests covering both
C# and F# features.

  ```bash
  make pldotnet-tests
  ```

- For C# tests only: Use this to specifically test C# related features.

  ```bash
    make csharp-tests
  ```

- For F# tests only: This target runs the F# functionalities.

  ```bash
  make fsharp-tests
  ```

### 1.5.2. SQL Tests

#### 1.5.2.1. Understanding tests results

While SQL tests in the pldotnet suite may not provide as detailed insights as
xUnit tests, they are invaluable for illustrating potential use cases and
serving as examples for the PostgreSQL community. Here's how to interpret and
 utilize the SQL test results effectively:

1. Direct Results: SQL tests primarily reveal discrepancies in expected versus
  actual outcomes directly through assertion results, which are recorded in
  a results table. This immediate feedback is useful for quick checks.
2. Limited Diagnostics: Unlike xUnit tests, SQL tests do not inherently provide
  detailed error messages or stack traces. Issues beyond simple assertion
  failures require a more hands-on approach to diagnose.

After the tests are executed, the results are tabulated and displayed in the
terminal, as illustrated below:

- Result Table: A table showing the feature, test name, and result (pass or fail)
  for each test is printed. Here's a sample format:

  ```
  | Feature       | Test Name             | Result |
  |---------------|-----------------------|--------|
  | c#-bit        | modifybit1            | t      |
  | c#-bit-null   | modifybit2            | t      |
  | c#-varbit     | modifyvarbit1         | t      |
  | c#-varbit-null| modifyvarbit2         | t      |
  | c#-varbit     | concatenatevarbit1    | t      |
  | c#-varbit     | concatenatevarbit2    | t      |
  ...
  ...
  ...
  ```

- Aggregated Results: Following the detailed table, a summarized count of
  passed and failed tests is provided, offering a quick overview of the test
  suite's health.

  ```
  | Result | Count |
  |--------|-------|
  | f      | 1     |
  | t      | 996   |
  ```

For tests that fail due to reasons other than a mismatched expected result
(e.g., compilation errors,runtime exceptions), further investigation is
required. During the test execution, detailed logs are stored in an
`automated_test_results` directory within the working directory. These files
 contain valuable information about the execution of each test.

#### 1.5.2.2. Running the tests

Direct SQL tests provide a low-level examination of pldotnet's behavior with
SQL operations. Execute these to run the tests in SQL:

- For both C# and F# tests: Run all the tests written in SQL for both C#
and F#.

  ```bash
  make pldotnet-tests-sql
  ```

- For C# tests only: Run the C# tests in SQL.

  ```bash
    make csharp-tests-sql
  ```

- For F# tests only: Execute the F# tests written in SQL.

  ```bash
  make fsharp-tests-sql
  ```

Utilize these commands to ensure the robustness and reliability of pldotnet
before deployment. For any issues or more detailed instructions, refer to the
corresponding test directories or contact the support team.
