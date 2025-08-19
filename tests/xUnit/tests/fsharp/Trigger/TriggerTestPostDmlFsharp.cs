using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestPostDmlFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTriggerTestPostDmlFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "noop",
            Arguments = new List<FunctionArgument>(),
            ReturnType = "VOID",
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
                "validRows",
                "NOT EXISTS (SELECT 1 FROM trigger_test_table WHERE id NOT IN (1,2,6,7,13))",
                null!
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTestPostDmlFsharp(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix
    )
    {
        var createFunctionSql = GetFunctionDefinition(FunctionInfo!);
        ExecuteSql(createFunctionSql);

        ExecuteSql(@"
            INSERT INTO trigger_test_table(id, message) VALUES
                (1, 'Inserted Text'),
                (2, 'Inserted Second Text'),
                (3, 'Inserted Third Text (for updating)'),
                (4, 'Inserted Fourth Text (for deletion)'),
                (5, 'Inserted Fifth Text (for simple skip)'),
                (6, 'Inserted Sixth Text (for values check modify)'),
                (7, 'This should be a string');

            UPDATE trigger_test_table
               SET message = 'Updated Text'
             WHERE id = 1;

            UPDATE trigger_test_table
               SET id = 13, message = 'Updated Third Text'
             WHERE id = 3;

            DELETE FROM trigger_test_table
                WHERE id = 4;

            DELETE FROM trigger_test_table
                WHERE id = 5;
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
public class TriggerTestPostDmlTestsFSharp : BaseTriggerTestPostDmlFsharpTests
{
    protected override string FunctionBody => "()";
    protected override LanguageType Language => LanguageType.PlfSharp;
}
