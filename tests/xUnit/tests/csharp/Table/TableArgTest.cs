using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseTableArgTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTableArgTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "TableArgTest",
            Arguments = new List<FunctionArgument> { new FunctionArgument("lim", "int4") },
            ReturnType = "TABLE(id integer, name text)",
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
            "data-integrity-1",
            "SUM(id) = 45",
            "TableArgTest(10)"
        };

        yield return new object[]
        {
            "c#-table-function",
            "data-integrity-2",
            "SUM(id) IS NULL",
            "TableArgTest(NULL::int)"
        };

        yield return new object[]
        {
            "c#-table-function",
            "data-integrity-3",
            "COUNT(*) = 0",
            "TableArgTest(NULL::int)"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTableArg(
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
public class TableArgTestsCsharp : BaseTableArgTests
{
    protected override string FunctionBody =>
        @"
return lim.HasValue
    ? Enumerable.Range(0, lim.Value).Select(i => ((int?)i, $""The number is {i}""))
    : Enumerable.Empty<(int? id, string? name)>();
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
