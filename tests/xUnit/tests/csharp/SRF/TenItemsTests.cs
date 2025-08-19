using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseTenItemsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseTenItemsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ten_items",
            Arguments = new List<FunctionArgument> { new FunctionArgument("arg", "text") },
            ReturnType = "SETOF text",
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
            "c#-srf-strings",
            "string-checksum-1",
            @"
WITH aggregated AS (
    SELECT string_agg(ten_items, '') AS concatenated_items
    FROM (SELECT ten_items('apples')) AS t
)
",
            "encode(digest(concatenated_items, 'sha256'), 'hex') = '94091910bae126a50dfb041cd9e9a44efd716c77185628b3bce7a5965a207555'",
            null!
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TenItemsTests(
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

[Trait("Language", "CSharp")]
[Trait("Category", "SRF")]
public class TenItemsTestsCsharp : BaseTenItemsTests
{
    protected override string FunctionBody =>
        @"
for(int i=1; i<=10; i++){ yield return $""{i} {arg}""; }
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}
