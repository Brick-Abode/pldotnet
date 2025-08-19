using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestSkipTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestSkipTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_skip",
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
                "skipWorks",
                "NOT EXISTS (SELECT 1 FROM trigger_test_table WHERE id = 5)",
                null!
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestSkip(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix)
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_2 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_2
    BEFORE INSERT ON trigger_test_table
    FOR EACH ROW
    WHEN (new.id = 5)
    EXECUTE FUNCTION trigger_test_skip('BEFORE/INSERT/ROW', '2');
";
        ExecuteSql(triggerSql);

        ExecuteSql(@"
            INSERT INTO trigger_test_table (id, message)
            VALUES (5, 'Inserted Fifth Text (for simple skip)');
        ");

        var cte = @"
WITH cte AS (
    SELECT 1
)
";

        RunTestWithSuffix(
            featureName:     featureName,
            testName:        testName,
            cteStatement:    cte,
            customAssertion: customAssertion,
            querySuffix:     querySuffix,
            forceCte:        true
        );
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Trigger")]
public class TriggerTestSkipTestsCSharp : BaseTriggerTestSkipTests
{
    protected override string FunctionBody => @"
    if (tg.Arguments[1] != ""2""){
        throw new SystemException($""Assertion failed: wrong trigger argument, '{tg.Arguments[1]}' != '2'"");
    }

    if((int)tg.NewRow[0] == 5) {
        return ReturnMode.TriggerSkip;
    }

    return ReturnMode.Normal;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}