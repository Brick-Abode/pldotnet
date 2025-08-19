using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestFunctionModifyTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTriggerTestFunctionModifyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "trigger_test_function_modify",
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
                "rowModifiedByTrigger",
                "message = 'MODIFIED Text!!!'",
                "WHERE id = 2"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTriggerTestFunctionModify(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo);
        ExecuteSql(createFunctionSql);

        var triggerSql = @"
DROP TRIGGER IF EXISTS test_trigger_BIR_1 ON trigger_test_table;
CREATE TRIGGER test_trigger_BIR_1
  BEFORE INSERT ON trigger_test_table
  FOR EACH ROW
  WHEN (NEW.id = 2)
  EXECUTE FUNCTION trigger_test_function_modify('BEFORE/INSERT/ROW','1');
";
        ExecuteSql(triggerSql);

        var cte = @"
WITH cte AS (
    INSERT INTO trigger_test_table (id, message)
    VALUES (2, 'Inserted Second Text')
    RETURNING *
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
public class TriggerTestFunctionModifyTestsCSharp : BaseTriggerTestFunctionModifyTests
{
    protected override string FunctionBody => @"
    if (tg.Arguments[1] != ""1""){
        throw new SystemException($""Assertion failed: wrong trigger argument, '{tg.Arguments[1]}' != '1'"");
    }

    if ((int)tg.NewRow[0] != 2) {
        throw new SystemException($""Assertion failed: wrong row value, {tg.NewRow[1]} != 2"");
    }

    tg.NewRow[1] = ""MODIFIED Text!!!"";
    return ReturnMode.TriggerModify;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}