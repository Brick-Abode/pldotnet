using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestSkipFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestSkipFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_skip_fsharp",
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
                "f#-trigger",
                "skipWorks",
                "NOT EXISTS (SELECT 1 FROM trigger_test_table WHERE id = 5)",
                null!
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestSkipFsharp(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo!);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_2 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_2
    BEFORE INSERT ON trigger_test_table
    FOR EACH ROW
    WHEN (new.id = 5)
    EXECUTE FUNCTION trigger_test_skip_fsharp('BEFORE/INSERT/ROW', '2');
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

[Trait("Language", "FSharp")]
[Trait("Category", "Trigger")]
public class TriggerTestSkipTestsFSharp : BaseTriggerTestSkipFsharpTests
{
    protected override string FunctionBody => @"
        if tg.Arguments.[1] <> ""2"" then
            raise (SystemException($""Assertion failed: wrong trigger argument, '{tg.Arguments.[1]}' != '2'""))

        if (unbox<int> tg.NewRow.[0]) = 5 then
            ReturnMode.TriggerSkip
        else
            ReturnMode.Normal;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}