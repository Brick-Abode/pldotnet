using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestTgValsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTriggerTestTgValsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_tg_vals",
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
                "tgValuesCorrect",
                "message = 'TG value assertions passed'",
                "WHERE id = 6"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestTgVals(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_3 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_3
    BEFORE INSERT ON trigger_test_table
    FOR EACH ROW
    WHEN (new.id = 6)
    EXECUTE FUNCTION trigger_test_tg_vals('BEFORE/INSERT/ROW', '3');
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

[Trait("Language", "CSharp")]
[Trait("Category", "Trigger")]
public class TriggerTestTgValsTestsCSharp : BaseTriggerTestTgValsTests
{
    protected override string FunctionBody => @"
    if((int)tg.NewRow[0] == 6) {
        if(
            (tg.TriggerName == ""test_trigger_bir_3"") &&
            (tg.TriggerWhen == ""BEFORE"") &&
            (tg.TriggerLevel == ""ROW"") &&
            (tg.TriggerEvent == ""INSERT"") &&
            (tg.RelationId > 0) &&
            (tg.TableName == ""trigger_test_table"") &&
            (tg.TableSchema == ""public"") &&
            (tg.NewRow.Length == 2) &&
            ((int)tg.NewRow[0] == 6) &&
            (tg.Arguments[0] == ""BEFORE/INSERT/ROW"") &&
            (tg.Arguments[1] == ""3"")
        )
        {
            tg.NewRow[1] = ""TG value assertions passed"";
            return ReturnMode.TriggerModify;
        }
    }
    Elog.Warning($""failed test, tg values didn't check out: {tg}"");
    return ReturnMode.Normal;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}