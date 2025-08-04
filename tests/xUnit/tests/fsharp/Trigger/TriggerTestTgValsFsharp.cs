using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestTgValsFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestTgValsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_tg_vals_fsharp",
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
                "tgValuesCorrect",
                "message = 'TG value assertions passed'",
                "WHERE id = 6"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestTgValsFsharp(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo!);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_3 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_3
    BEFORE INSERT ON trigger_test_table
    FOR EACH ROW
    WHEN (new.id = 6)
    EXECUTE FUNCTION trigger_test_tg_vals_fsharp('BEFORE/INSERT/ROW', '3');
";
        ExecuteSql(triggerSql);

        var cte = @"
WITH cte AS (
    INSERT INTO trigger_test_table
    VALUES (6, 'Inserted Sixth Text (for values check modify)')
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
public class TriggerTestTgValsTestsFSharp : BaseTriggerTestTgValsFsharpTests
{
    protected override string FunctionBody => @"
        if unbox<int> tg.NewRow.[0] = 6 then
            if tg.TriggerName = ""test_trigger_bir_3"" &&
                tg.TriggerWhen = ""BEFORE"" &&
                tg.TriggerLevel = ""ROW"" &&
                tg.TriggerEvent = ""INSERT"" &&
                tg.RelationId > 0 &&
                tg.TableName = ""trigger_test_table"" &&
                tg.TableSchema = ""public"" &&
                tg.NewRow.Length = 2 &&
                unbox<int> tg.NewRow.[0] = 6 &&
                tg.Arguments.[0] = ""BEFORE/INSERT/ROW"" &&
                tg.Arguments.[1] = ""3"" then
                    tg.NewRow.[1] <- ""TG value assertions passed""
                    ReturnMode.TriggerModify
            else
                Elog.Error($""failed test, tg values didn't check out: {tg}"")
                ReturnMode.Normal
        else
            Elog.Error($""failed test, tg values didn't check out: {tg}"")
            ReturnMode.Normal;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}