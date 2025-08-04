using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseTableArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTableArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "TableArrayTest",
            Arguments = new List<FunctionArgument> { new FunctionArgument("lim", "int4") },
            ReturnType = "TABLE(id integer[], name text)",
            Body = FunctionBody,
            Language = Language,
            IsStrict = false,
        };
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[]
        {
            "c#-table-function",
            "array-output-1",
            "COUNT(*) = 5",
            "TableArrayTest(5)"
        };

        yield return new object[]
        {
            "c#-table-function",
            "array-output-2",
            "SUM(val) = 110",
            "TableArrayTest(5), UNNEST(id) AS val"
        };

        yield return new object[]
        {
            "c#-table-function",
            "array-output-3",
            "SUM(val) = 110",
            @"TableArrayTest(5), UNNEST(id) AS val
            WHERE ARRAY_POSITION(id, NULL) = 3"
        };

        yield return new object[]
        {
            "c#-table-function",
            "array-output-4",
            "COALESCE(SUM(val) = 110, true)",
            @"TableArrayTest(5), UNNEST(id) AS val 
            WHERE ARRAY_POSITION(id, NULL) = 2"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTableArray(
        string featureName,
        string testName,
        string customAssertion,
        string querySuffix,
        bool forceCte = true,
        string cteStatement = ""
    )
    {
        RunTestWithSuffix(
            featureName,
            testName,
            cteStatement,
            customAssertion,
            querySuffix,
            forceCte
        );
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Table")]
public class TableArrayTestsCsharp : BaseTableArrayTests
{
    protected override string FunctionBody =>
        $@"
            return lim.HasValue
                ? Enumerable.Range(0, lim.Value).Select(i => ((Array)new int?[] {{ i, i*i*i, null }}, $""The number is {{i}}""))
                : Enumerable.Empty<(Array? id, string? name)>();
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
