using System;
using System.Collections.Generic;
using Xunit;

public abstract class BaseTriggerTestRowUpdateFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTriggerTestRowUpdateFsharpTests()
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
                "rowUpdated",
                "message = 'Updated Text'",
                "WHERE id = 1"
            },
            new object[]
            {
                "f#-trigger",
                "rowUpdated-2",
                "message = 'Updated Third Text'",
                "WHERE id = 13"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestRowUpdateFsharp(
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
                (3, 'Inserted Third Text (for updating)');

            UPDATE trigger_test_table
               SET message = 'Updated Text'
             WHERE id = 1;

            UPDATE trigger_test_table
               SET id = 13, message = 'Updated Third Text'
             WHERE id = 3;
        ");

        var cte = @"
WITH cte AS (
    SELECT id, message
      FROM trigger_test_table
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
public class TriggerTestRowUpdateTestsFSharp : BaseTriggerTestRowUpdateFsharpTests
{
    protected override string FunctionBody => "()";
    protected override LanguageType Language => LanguageType.PlfSharp;
}
