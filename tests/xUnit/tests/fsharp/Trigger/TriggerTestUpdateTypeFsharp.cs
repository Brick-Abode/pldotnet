using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestUpdateTypeFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestUpdateTypeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_update_type_fsharp",
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
                "tgTypeMismatchHandling",
                "message = 'This should be a string'",
                "WHERE id = 7"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestUpdateTypeFsharp(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo!);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_4 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_4
    BEFORE INSERT ON trigger_test_table
    FOR EACH ROW
    WHEN (new.id = 7)
    EXECUTE FUNCTION trigger_test_update_type_fsharp('BEFORE/INSERT/ROW', '4');
";
        ExecuteSql(triggerSql);

        var cte = @"
WITH cte AS (
    INSERT INTO trigger_test_table (id, message)
    VALUES (7, 'This should be a string')
    RETURNING id, message
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
public class TriggerTestUpdateTypeTestsFSharp : BaseTriggerTestUpdateTypeFsharpTests
{
    protected override string FunctionBody => @"
        tg.NewRow.[1] = 1;
        ReturnMode.TriggerModify;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}