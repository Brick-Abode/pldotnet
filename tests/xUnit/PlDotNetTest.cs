using DotNetEnv;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Xunit;

[Collection("Sequential")]
public class PlDotNetTest
{
    protected SqlFunctionInfo? FunctionInfo;

    string DatabaseConnectionString =
        Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") + ";Include Error Detail=true"
        ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not set.");

    int TestCount = 0;

    public enum LanguageType
    {
        PlcSharp,
        PlfSharp,
        PlPython,
        PlR,
        PlPgSQL,
        PlV8,
        PlTCL,
        PlLua,
        PlPerl
    }

    public enum SqlTestType
    {
        Function,
        Procedure,
        DoBlock
    }

    public Dictionary<SqlTestType, string> TestTypeMap = new Dictionary<SqlTestType, string>
    {
        { SqlTestType.Function, "FUNCTION" },
        { SqlTestType.Procedure, "PROCEDURE" },
        { SqlTestType.DoBlock, "DO BLOCK" }
    };

    public class SqlFunctionInfo
    {
        // New properties for dynamic SQL construction

        public string CteStatement { get; set; } = string.Empty;
        public string CustomAssertion { get; set; } = string.Empty; // For complex assertion logic
        public string QuerySuffix { get; set; } = string.Empty; // For additional WHERE, LIMIT, etc.
        public SqlTestType TestType { get; set; } = SqlTestType.Function;
        public string Name { get; set; } = string.Empty;
        public List<FunctionArgument> Arguments { get; set; } = new List<FunctionArgument>();
        public string ReturnType { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public LanguageType Language { get; set; }
        public bool IsStrict { get; set; }
        public string FunctionName { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public string FeatureName { get; set; } = string.Empty;
        public string InputStr { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public string CastFunctionAs { get; set; } = string.Empty;

        public int? TestId { get; set; }

        public bool FunctionCreatedSuccessfully { get; set; } = false;

        public bool TestInsertedSuccessfully { get; set; } = false;

        public string SqlFunctionDefinition { get; set; } = string.Empty;

        public string SqlFunctionCall { get; set; } = string.Empty;

        public string LanguageString
        {
            get
            {
                return Language switch
                {
                    LanguageType.PlcSharp => "plcsharp",
                    LanguageType.PlfSharp => "plfsharp",
                    LanguageType.PlPython => "plpython3u",
                    LanguageType.PlR => "plr",
                    LanguageType.PlPgSQL => "plpgsql",
                    LanguageType.PlV8 => "plv8",
                    LanguageType.PlTCL => "pltcl",
                    LanguageType.PlLua => "pllua",
                    LanguageType.PlPerl => "plperl",
                    _ => throw new System.NotImplementedException()
                };
            }
        }

        public SqlFunctionInfo() { }
    }

    public class FunctionArgument
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public FunctionArgument(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    public class TestQueryParameters
    {
        public List<(string Name, string Query)> Ctes { get; set; } =
            new List<(string Name, string Query)>();
        public string TestCategory { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public string Assertion { get; set; } = string.Empty;
        public string FinalSelect { get; set; } = string.Empty;
    }

    /// <summary>
    /// Constructs and returns the SQL definition of a function based on the provided SqlFunctionInfo.
    /// </summary>
    /// <param name="functionInfo">The information about the SQL function.</param>
    /// <returns>A string representing the SQL definition of the function.</returns>
    /// <remarks>
    /// The method uses the properties of the SqlFunctionInfo object to create a SQL CREATE OR REPLACE FUNCTION statement.
    /// If the IsStrict property of the SqlFunctionInfo object is true, the "STRICT" keyword is added to the SQL definition.
    /// </remarks>
    public virtual string GetFunctionDefinition(SqlFunctionInfo functionInfo)
    {
        var arguments = string.Join(
            ", ",
            functionInfo.Arguments.Select(arg => $"{arg.Name} {arg.Type}")
        );

        string methodKeyword =
            functionInfo.TestType == SqlTestType.Function ? "FUNCTION" : "PROCEDURE";

        string strictKeyword = functionInfo.IsStrict ? "STRICT" : "";

        // Conditionally build the returnTypeString
        string returnTypeString = string.IsNullOrEmpty(functionInfo.ReturnType)
            ? string.Empty
            : $"RETURNS {functionInfo.ReturnType}";

        return $@"CREATE OR REPLACE {methodKeyword} {functionInfo.Name}({arguments})
{returnTypeString} AS $$
    {functionInfo.Body}
$$ LANGUAGE {functionInfo.LanguageString} {strictKeyword};";
    }

    public virtual string GetFunctionCall(SqlFunctionInfo functionInfo, bool forceCte = false)
    {
        if (functionInfo.TestType == SqlTestType.Procedure)
        {
            return $"CALL {functionInfo.Name}({functionInfo.InputStr});";
        }

        return TestResultStrategyFactory.GetStrategy(functionInfo, forceCte).BuildInsertSql(functionInfo);
    }

    public interface ITestResultStrategy
    {
        // AppliesTo determines if the strategy should be used for the given functionInfo
        bool AppliesTo(SqlFunctionInfo functionInfo);

        // BuildInsertSql returns the SQL code to insert the test result
        string BuildInsertSql(SqlFunctionInfo functionInfo);
    }

    public class DefaultTestResultStrategy : ITestResultStrategy
    {
        public bool AppliesTo(SqlFunctionInfo functionInfo)
        {
            return true;
        }

        public string BuildInsertSql(SqlFunctionInfo functionInfo)
        {
            string functionCall = $"{functionInfo.Name}({functionInfo.InputStr})";

            string cast = !string.IsNullOrWhiteSpace(functionInfo.CastFunctionAs)
                ? $"CAST({functionCall} AS {functionInfo.CastFunctionAs})"
                : functionCall;

            string nullComparison =
                functionInfo.ExpectedResult == "= null"
                    ? "IS NULL"
                    : $"{cast} {functionInfo.ExpectedResult}";

            if (string.IsNullOrWhiteSpace(functionInfo.ExpectedResult))
            {
                nullComparison = "IS NOT NULL";
            }

            string sqlCode =
                $@"INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
                            VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', {nullComparison})
                            RETURNING id;";

            return sqlCode;
        }
    }

    //     public class CteTestResultStrategy : ITestResultStrategy
    //     {
    //         public bool AppliesTo(SqlFunctionInfo functionInfo)
    //         {
    //             // This strategy applies if a CTE statement is defined
    //             return !string.IsNullOrWhiteSpace(functionInfo.CteStatement);
    //         }

    //         public string BuildInsertSql(SqlFunctionInfo functionInfo)
    //         {
    //             // Use the provided CTE statement directly in the SQL command construction
    //             string sqlCode =
    //                 $@"
    // {functionInfo.CteStatement}
    // INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
    // SELECT '{functionInfo.FeatureName}', '{functionInfo.TestName}', {functionInfo.CustomAssertion} FROM cte {functionInfo.QuerySuffix} RETURNING id;";

    //             return sqlCode;
    //         }
    //     }

    // Defines a class CteTestResultStrategy that implements the ITestResultStrategy interface.
    public class CteTestResultStrategy : ITestResultStrategy
    {
        // Determines if the strategy applies to the given SQL function based on the presence of a CTE statement.

        public bool AppliesTo(SqlFunctionInfo functionInfo)
        {
            // Returns true if the CteStatement property of functionInfo is not null or whitespace, indicating
            // this strategy should be used for functions with CTEs.
            return !string.IsNullOrWhiteSpace(functionInfo.CteStatement);
        }

        // Builds an SQL INSERT statement using the information provided in the SqlFunctionInfo parameter.
        public string BuildInsertSql(SqlFunctionInfo functionInfo)
        {
            // Extracts the name of the CTE from its SQL statement to use in the final query.
            string cteName = ExtractCteName(functionInfo.CteStatement)!;

            // Constructs an SQL statement that includes the CTE statement and an INSERT INTO operation.
            // The INSERT operation adds a new row into the automated_test_results table with details from the functionInfo parameter
            // and selects values from the CTE defined earlier, applying any specified query suffix. Finally, it returns the id of the inserted row.
            string sqlCode =
                $@"
{functionInfo.CteStatement}
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT '{functionInfo.FeatureName}', '{functionInfo.TestName}', {functionInfo.CustomAssertion} FROM {cteName} {functionInfo.QuerySuffix} RETURNING id;";

            // Returns the constructed SQL code.
            return sqlCode;
        }

        // A private helper method to extract the name of the CTE from its SQL statement.
        private string? ExtractCteName(string cteStatement)
        {
            // Uses a regular expression to find the CTE name in the CTE statement by looking for the pattern
            // that follows "WITH" and precedes "AS". Assumes the CTE name is a single word (\w+).
            var match = System.Text.RegularExpressions.Regex.Match(
                cteStatement,
                @"WITH\s+(\w+)\s+AS"
            );
            // If the pattern is found, the CTE name is returned.
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            // If the pattern is not found, returns null indicating no CTE name could be extracted.
            return null;
        }
    }

    //     public class CteTestResultStrategy : ITestResultStrategy
    //     {
    //         public bool AppliesTo(SqlFunctionInfo functionInfo)
    //         {
    //             // This strategy applies if a CTE statement is defined and not empty.
    //             return !string.IsNullOrWhiteSpace(functionInfo.CteStatement);
    //         }

    //         public string BuildInsertSql(SqlFunctionInfo functionInfo)
    //         {
    //             // Directly prepend the CTE statement to the SQL command
    //             string sqlCode =
    //                 $@"
    // {functionInfo.cteStatement}
    // INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
    // SELECT '{functionInfo.FeatureName}', '{functionInfo.TestName}', {functionInfo.CustomAssertion} RETURNING id;";

    //             return sqlCode;
    //         }
    //     }

    public class JsonOrXmlTestResultStrategy : ITestResultStrategy
    {
        public bool AppliesTo(SqlFunctionInfo functionInfo)
        {
            return (
                    functionInfo.ReturnType != null
                    && (
                        functionInfo.ReturnType.IndexOf("JSON", StringComparison.OrdinalIgnoreCase)
                            >= 0
                        || functionInfo.ReturnType.IndexOf(
                            "XML",
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0
                    )
                )
                || (
                    functionInfo.ExpectedResult != null
                    && functionInfo.ExpectedResult.IndexOf(
                        "::JSON::TEXT",
                        StringComparison.OrdinalIgnoreCase
                    ) >= 0
                );
        }

        public string BuildInsertSql(SqlFunctionInfo functionInfo)
        {
            string functionCall = $"{functionInfo.Name}({functionInfo.InputStr})::TEXT";
            string expectedResult = functionInfo.ExpectedResult + "::TEXT";

            string sqlCode =
                $@"   INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
            VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', {functionCall} {expectedResult})
            RETURNING id;";
            return sqlCode;
        }
    }

    public class TestResultStrategyFactory
    {
        private static readonly List<ITestResultStrategy> Strategies = new List<ITestResultStrategy>
        {
            new CteTestResultStrategy(),
            new JsonOrXmlTestResultStrategy(),
            new DefaultTestResultStrategy()
        };

        public static ITestResultStrategy GetStrategy(
            SqlFunctionInfo functionInfo,
            bool forceCte = false
        )
        {
            if (forceCte)
            {
                return Strategies[0];
            }
            return Strategies.FirstOrDefault(strategy => strategy.AppliesTo(functionInfo))
                ?? new DefaultTestResultStrategy();
        }
    }

    /// <summary>
    /// Executes the provided SQL command that is expected to return an integer ID.
    /// This is typically used with SQL INSERT commands that use the RETURNING clause
    /// to return the ID of the newly inserted row.
    /// </summary>
    /// <param name="sqlCode">The SQL code to execute.</param>
    /// <returns>The retrieved integer ID if the command executed successfully and returned a value,
    /// null otherwise.</returns>
    protected int? ExecuteSqlReturnId(string sqlCode)
    {
        StringBuilder messages = new StringBuilder();
        Exception exception = null!;

        try
        {
            using var connection = new NpgsqlConnection(DatabaseConnectionString);
            connection.Open();
            connection.Notice += (sender, e) =>
            {
                messages.AppendLine($"{e.Notice.MessageText}");
            };
            using var command = new NpgsqlCommand(sqlCode, connection);
            command.CommandType = CommandType.Text;
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            return null;
        }
        catch (Exception ex)
        {
            exception = ex;
            return null;
        }
        finally
        {
            LogTestExecutionDetails(exception, messages);
        }
    }

    /// <summary>
    /// Fetches the test result for the given function info based on its test ID.
    /// </summary>
    /// <param name="functionInfo">Information about the test whose result is to be fetched.</param>
    /// <returns>The test result if found; null otherwise.</returns>
    public bool? FetchTestResult(SqlFunctionInfo functionInfo)
    {
        if (!functionInfo.TestId.HasValue)
        {
            // Console.WriteLine("TestId is not set.");
            return null;
        }

        string sqlSelect =
            $@"
SELECT RESULT
FROM automated_test_results
WHERE id = {functionInfo.TestId.Value};";

        object? result = ExecuteSql(sqlSelect);

        if (result is bool booleanResult)
        {
            return booleanResult;
        }

        return null;
    }

    /// <summary>
    /// Logs the execution details of a test, including any exception and SQL messages.
    /// </summary>
    /// <param name="exception">The exception that occurred during the test execution.</param>
    /// <param name="elogMessages">The SQL messages generated during the test execution.</param>
    protected void LogTestExecutionDetails(Exception exception, StringBuilder elogMessages)
    {
        if (elogMessages.Length > 0 || exception != null)
        {
            Console.WriteLine($"[START SQL EXECUTION DETAILS - TEST {TestCount}]");
            if (exception != null)
            {
                Console.WriteLine($"SQL Error:\n{exception.Message}");
            }
            if (elogMessages.Length > 0)
            {
                Console.WriteLine($"SQL Messages:\n{elogMessages.ToString().Trim()}");
            }
            Console.WriteLine($"[END SQL EXECUTION DETAILS - TEST {TestCount}]");
        }
    }

    /// <summary>
    /// Executes the provided SQL command.
    /// It returns true for successful execution
    /// and false for failures.
    /// </summary>
    /// <param name="sqlCode">The SQL code to execute.</param>
    /// <returns>It returns true for successful execution,
    /// and false otherwise.</returns>
    protected bool ExecuteSql(string sqlCode)
    {
        StringBuilder messages = new StringBuilder();
        Exception exception = null!;

        try
        {
            using var connection = new NpgsqlConnection(DatabaseConnectionString);
            connection.Open();
            connection.Notice += (sender, e) =>
            {
                messages.AppendLine($"{e.Notice.MessageText}");
            };

            using var command = new NpgsqlCommand(sqlCode, connection);
            // command.CommandType = CommandType.Text;
            if (sqlCode.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    object value = reader.GetValue(0);
                    if (value is bool booleanValue)
                    {
                        return booleanValue;
                    }
                }
                return false;
            }
            else
            {
                command.ExecuteNonQuery();
                return true;
            }
        }
        catch (Exception ex)
        {
            exception = ex;
            return false;
        }
        finally
        {
            LogTestExecutionDetails(exception, messages);
        }
    }

    /// <summary>
    /// Executes a generic test for the provided SQL function, logs the test result, and asserts the success of each step.
    /// <para>This method performs the following actions and validations:</para>
    /// <para>1. Sets up the function information based on the provided parameters.</para>
    /// <para>2. Attempts to define the function in the database.</para>
    /// <para>3. Validates (via assertion) that the function was successfully created.</para>
    /// <para>4. Inserts the test result for the defined function into the database.</para>
    /// <para>5. Validates (via assertion) that the test result was successfully inserted.</para>
    /// <para>6. Fetches the test result from the database.</para>
    /// <para>7. Validates (via assertion) that a test result was retrieved.</para>
    /// <para>8. Validates (via assertion) that the retrieved test result matches the expected result.</para>
    /// </summary>
    /// <param name="functionName">Name of the SQL function to test.</param>
    /// <param name="featureName">Feature associated with the test.</param>
    /// <param name="input">Input string for the SQL function.</param>
    /// <param name="expectedResult">Expected result string for the SQL function.</param>
    public void RunGenericTest(
        string featureName,
        string testName,
        string input,
        string expectedResult
    )
    {
        RunTest(
            featureName: featureName,
            testName: testName,
            input: input,
            expectedResult: expectedResult
        );
    }

    public void RunTestWithSuffix(
        string featureName,
        string testName,
        string cteStatement,
        string customAssertion,
        string querySuffix = null!,
        bool forceCte = false
    )
    {
        RunTest(
            featureName: featureName,
            testName: testName,
            cteStatement: cteStatement,
            customAssertion: customAssertion,
            querySuffix: querySuffix,
            forceCte: forceCte
        );
    }

    public void RunTest(
        string featureName,
        string testName,
        string input = "",
        string expectedResult = "",
        string cteStatement = "",
        string customAssertion = "",
        string querySuffix = "",
        bool forceCte = false
    )
    {
        TestCount++;
        Console.WriteLine(
            $"[START TEST {TestCount}] Running test {testName} for feature {featureName}."
        );

        if (FunctionInfo == null)
        {
            Assert.Fail("FunctionInfo is null, test cannot proceed.");
            return;
        }

        // Set up the FunctionInfo object with the provided parameters
        FunctionInfo.TestName = testName;
        FunctionInfo.FeatureName = featureName;
        FunctionInfo.InputStr = input;
        FunctionInfo.ExpectedResult = expectedResult;
        FunctionInfo.CteStatement = cteStatement; // Updated to use the full CTE statement
        FunctionInfo.CustomAssertion = customAssertion;
        FunctionInfo.QuerySuffix = querySuffix;

        // Combine pieces of FunctionInfo to create SQL codes
        FunctionInfo.SqlFunctionDefinition = GetFunctionDefinition(FunctionInfo);
        FunctionInfo.SqlFunctionCall = GetFunctionCall(FunctionInfo, forceCte);
        Console.WriteLine(
            $"[START SQL FUNCTION - TEST {TestCount}]\n```sql\n{FunctionInfo.SqlFunctionDefinition}\n```\n[END SQL FUNCTION - TEST {TestCount}]\n"
        );
        Console.WriteLine(
            $"[START SQL CALL FUNCTION - TEST {TestCount}]\n```sql\n{FunctionInfo.SqlFunctionCall}\n```\n[END SQL CALL FUNCTION - TEST {TestCount}]"
        );

        // Create the function in the PostgreSQL database
        FunctionInfo.FunctionCreatedSuccessfully = ExecuteSql(FunctionInfo.SqlFunctionDefinition);
        Assert.True(
            FunctionInfo.FunctionCreatedSuccessfully,
            $"[COMPILATION ERROR] Test {TestCount} failed to create function in the PostgreSQL database."
        );

        if (FunctionInfo.TestType == SqlTestType.Procedure)
        {
            bool? procedureResult = ExecuteSql(FunctionInfo.SqlFunctionCall);
            Assert.True(
                procedureResult.HasValue && procedureResult.Value,
                $"[EXECUTION ERROR] Test {TestCount} failed to execute procedure test."
            );
            Console.WriteLine($"[END TEST {TestCount}] Test {testName} executed successfully.\n");
            return;
        }

        // Call test and insert the test result into the PostgreSQL table
        FunctionInfo.TestId = ExecuteSqlReturnId(FunctionInfo.SqlFunctionCall);

        Assert.True(
            FunctionInfo.TestId.HasValue,
            $"[EXECUTION ERROR] Test {TestCount} failed to execute test and insert test result into table."
        );

        // Fetch the test result from the PostgreSQL table and validate it
        bool? testResult = FetchTestResult(FunctionInfo);
        Assert.True(
            testResult.HasValue,
            $"[UNEXPECTED ERROR] Test {TestCount} failed to get the test result value."
        );
        Assert.True(
            testResult.Value,
            $"[ASSERTION ERROR] Test {TestCount} returned unexpected result."
        );

        Console.WriteLine($"[END TEST {TestCount}] Test {testName} executed successfully.\n");
    }
}
