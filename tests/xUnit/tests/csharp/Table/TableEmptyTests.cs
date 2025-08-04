using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseTableEmptyTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    public BaseTableEmptyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "TableEmptyTest",
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
            "empty-result-1",
            "COUNT(*) = 0",
            "TableEmptyTest(5)"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTableEmpty(
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
public class TableEmptyTestsCsharp : BaseTableEmptyTests
{
    protected override string FunctionBody =>
        @"
 yield break;
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
