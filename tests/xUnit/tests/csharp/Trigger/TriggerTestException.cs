using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

public abstract class BaseTriggerTestExceptionTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestExceptionTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_exception",
            Arguments = new List<FunctionArgument>(),
            ReturnType = "TRIGGER",
            Body = FunctionBody,
            Language = Language,
            IsStrict = false
        };
    }

    public static object[][] TestCases()
    {
        return new[]
        {
            new object[]
            {
                "c#-trigger",
                "exceptionThrown",
                @"
INSERT INTO trigger_test_table(id, message) VALUES (1, 'Initial');
UPDATE trigger_test_table SET message = 'Changed' WHERE id = 1;
"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerException(string featureName, string testName, string cteStatement)
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_AUS_4 ON trigger_test_table;
CREATE TRIGGER test_trigger_AUS_4
    AFTER UPDATE ON trigger_test_table
    FOR EACH STATEMENT
    EXECUTE FUNCTION trigger_test_exception ('AFTER/UPDATE/STATEMENT', '4');
";
        ExecuteSql(triggerSql);

        try
        {
            // This is going to fail internally and it will throw TrueException
            RunTestWithSuffix(
                featureName:     featureName,
                testName:        testName,
                cteStatement:    cteStatement,
                customAssertion: null!,
                querySuffix:     null!,
                forceCte:        false
            );

            // If the test gets here, the exception didn't happen and we force the failing
            Assert.Fail("Expected a trigger execution, but none was thrown.");
        }
        catch (Xunit.Sdk.TrueException)
        {
            // Capture the fail of harness, then register "success" manually
            var insertSql = $@"
    INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
    VALUES ('{featureName}', '{testName}', TRUE) RETURNING id;
    ";

            var testId = ExecuteSqlReturnId(insertSql);
            Assert.True(testId.HasValue, "Fail to insert the result of 'exceptionThrown'.");
        }
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Trigger")]
public class TriggerTestExceptionTestsCSharp : BaseTriggerTestExceptionTests
{
    protected override string FunctionBody => @"
    throw new SystemException(""This is a test of exception handling"");
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}