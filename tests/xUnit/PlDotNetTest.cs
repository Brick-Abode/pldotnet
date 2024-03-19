using System.Data;
using Npgsql;
using DotNetEnv;
using System.Collections.Generic;
using Xunit;
using System.Collections;

[Collection("Sequential")]
public class PlDotNetTest
{
    protected SqlFunctionInfo? FunctionInfo;

    public enum LanguageType
    {
        PlcSharp,
        PlfSharp
    }

    public enum SqlTestType
    {
        Function,
        Procedure,
        DoBlock
    }

    public class SqlFunctionInfo
    {
        public SqlTestType TestType { get; set; } = SqlTestType.Function;
        public string Name { get; set; } = string.Empty;
        public List<FunctionArgument> Arguments { get; set; } = new List<FunctionArgument>();
        public string ReturnType { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public LanguageType Language { get; set; }
        public bool IsStrict { get; set; }
        public string? FunctionName { get; set; }
        public string? TestName { get; set; }
        public string? FeatureName { get; set; }
        public string? InputStr { get; set; }
        public string? ExpectedResult { get; set; }
        public string? CastFunctionAs { get; set; } = "";


        public int? TestId { get; set; }

        public bool FunctionCreatedSuccessfully { get; set; } = false;

        public bool TestInsertedSuccessfully { get; set; } = false;

        public string LanguageString
        {
            get
            {
                return Language == LanguageType.PlcSharp ? "plcsharp" : "plfsharp";
            }
        }

        public SqlFunctionInfo()
        { }
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
        var arguments = string.Join(", ", functionInfo.Arguments.Select(arg => $"{arg.Name} {arg.Type}"));

        string methodKeyword = functionInfo.TestType == SqlTestType.Function ? "FUNCTION" : "PROCEDURE";

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


    /// <summary>
    /// Attempts to define a SQL function based on the provided function info.
    /// </summary>
    /// <param name="functionInfo">Information about the SQL function to define.</param>
    /// <returns>True if the function was defined successfully; false otherwise.</returns>
    public bool DefineFunction(SqlFunctionInfo functionInfo)
    {
        string sqlFunctionDefinition = GetFunctionDefinition(functionInfo);


        return ExecuteSql(sqlFunctionDefinition);
    }

    /// <summary>
    /// Inserts a test result into the automated_test_results table and sets the test ID in the function info.
    /// </summary>
    /// <param name="functionInfo">Information about the test and its result.</param>
    /// <returns>True if the test result was inserted successfully; false otherwise.</returns>

    public virtual bool ExecuteProcedureTests(SqlFunctionInfo functionInfo)
    {
        string sqlCode;

        sqlCode = $@"CALL {functionInfo.Name}({functionInfo.InputStr});";
        return ExecuteSql(sqlCode);
    }

    public virtual bool InsertTestResult(SqlFunctionInfo functionInfo)
    {
        string sqlCode;
        string functionCall = $"{functionInfo.Name}({functionInfo.InputStr})";

        bool isReturnTypeJsonOrXml = functionInfo.ReturnType != null &&
                                    (functionInfo.ReturnType == "XML[]" ||
                                    functionInfo.ReturnType == "JSON[]" ||
                                    functionInfo.ReturnType == "JSON[][][]");

        bool isExpectedResultJsonText = functionInfo.ExpectedResult != null &&
                                        functionInfo.ExpectedResult.Contains("::JSON::TEXT");

        if (isReturnTypeJsonOrXml || isExpectedResultJsonText)
        {
            functionCall += "::TEXT";
            functionInfo.ExpectedResult += "::TEXT";
        }

        if (!functionInfo.FunctionCreatedSuccessfully)
        {
            sqlCode = $@"INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
            VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', false)
            RETURNING id;";
        }
        else
        {
            if (functionInfo.ExpectedResult == "= null")
            {
                sqlCode = $@"INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
                VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', CASE WHEN {functionCall} IS NULL THEN true ELSE false END)
                RETURNING id;";
            }
            else
            {
                if (functionInfo.CastFunctionAs == "")
                {
                    sqlCode = $@"INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
                    VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', {functionCall} {functionInfo.ExpectedResult})
                    RETURNING id;";
                }
                else
                {
                    sqlCode = $@"INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
                    VALUES ('{functionInfo.FeatureName}', '{functionInfo.TestName}', CAST({functionCall} AS {functionInfo.CastFunctionAs}) {functionInfo.ExpectedResult})
                    RETURNING id;";
                }
            }
        }

        int? returnedId = ExecuteSqlReturnId(sqlCode);
        if (returnedId.HasValue)
        {
            functionInfo.TestId = returnedId.Value;
            return true;
        }
        return false;
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
        try
        {
            // Retrieve database connection string from environment variables
            string databaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
                                              ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not set.");

            // Establish a connection to the database
            using var connection = new NpgsqlConnection(databaseConnectionString);
            connection.Open();

            // Prepare the SQL command to be executed
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
            Console.WriteLine($"SQL execution failed: {ex.Message}");
            return null;
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
            Console.WriteLine("TestId is not set.");
            return null;
        }

        string sqlSelect = $@"
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
    /// Executes the provided SQL command.
    /// It returns true for successful execution
    /// and false for failures.
    /// </summary>
    /// <param name="sqlCode">The SQL code to execute.</param>
    /// <returns>It returns true for successful execution,
    /// and false otherwise.</returns>
    protected bool ExecuteSql(string sqlCode)
    {
        try
        {
            string databaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
                                              ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not set.");

            using (var connection = new NpgsqlConnection(databaseConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sqlCode, connection))
                {
                    command.CommandType = CommandType.Text;

                    if (sqlCode.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                object value = reader.GetValue(0);


                                if (value is bool booleanValue)
                                {
                                    return booleanValue;
                                }
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SQL execution failed: {ex.Message}");
            return false;
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
    public void RunGenericTest(string featureName, string testName, string input, string expectedResult)
    {
        if (FunctionInfo == null) return;
        FunctionInfo.TestName = testName;
        FunctionInfo.FeatureName = featureName;
        FunctionInfo.InputStr = input;
        FunctionInfo.ExpectedResult = expectedResult;

        FunctionInfo.FunctionCreatedSuccessfully = DefineFunction(FunctionInfo);


        if (FunctionInfo.TestType == SqlTestType.Procedure)
        {
            bool? procedureResult = ExecuteProcedureTests(FunctionInfo);
            Assert.True(procedureResult, "Failed at the Call Procedure step.");
            return;
        }
        bool testInsertionResult = InsertTestResult(FunctionInfo);

        bool? testResult = FetchTestResult(FunctionInfo);
        Assert.True(FunctionInfo.FunctionCreatedSuccessfully, "Failed to create function in .NET");
        Assert.True(testInsertionResult, "Failed to execute the function.");
        Assert.True(testResult.HasValue, "Failed to get the test result value.");
        if (!testResult.HasValue) return;
        Assert.True(testResult.Value, "Test did not return the expected value.");
    }
}
