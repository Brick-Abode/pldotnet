using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseStringToIntegerArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseStringToIntegerArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "StringToIntegerArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("args", "text[]") },
            ReturnType = "SETOF integer",
            Body = FunctionBody,
            Language = Language,
            IsStrict = false,
        };
    }

    protected void SetupTest(string cteStatement)
    {
        this.cteStatement = cteStatement;
        FunctionInfo!.CteStatement = cteStatement;
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[]
        {
            "c#-srf-comparison",
            "comparison-1",
            @"WITH data1 AS (
    WITH data2 AS (
        SELECT ARRAY ['1', '0 ', '-3', NULL::text, '99'] as input
    )
    SELECT
        StringToIntegerArray(input) AS col1,
        string_to_integer_array_plsql(input) AS col2
    FROM data2
)",
            "bool_and(COALESCE(col1 = col2, false)) AS all_equal"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestStringToIntegerArray(
        string featureName,
        string testName,
        string cteStatement,
        string customAssertion,
        string querySuffix = null!
    )
    {
        SetupTest(cteStatement);
        RunTestWithSuffix(featureName, testName, this.cteStatement, customAssertion, querySuffix);
    }
}

[Trait("Language", "Csharp")]
[Trait("Category", "SRF")]
public class StringToIntegerArrayTestsCsharp : BaseStringToIntegerArrayTests
{
    protected override string FunctionBody =>
        @"
        return args.Cast<string>().Select(arg => (int?)(arg == null ? 0 : int.Parse(arg)));
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}

// [Trait("Language", "SQL")]
// [Trait("Category", "SRF")]
// public class StringToIntegerArrayPlSqlTestsSQL : BaseStringToIntegerArrayPlSqlTests
// {
//     protected override string FunctionBody =>
//         @"
// DECLARE
//       value text;
//     BEGIN
//       FOREACH value IN ARRAY strings
//       LOOP
//         IF value IS NULL THEN
//           RETURN NEXT 0;
//         ELSE
//           RETURN NEXT value::integer;
//         END IF;
//       END LOOP;
//       RETURN;
//     END;
//     ";
//     protected override LanguageType Language => LanguageType.PlPgSQL;
// }
