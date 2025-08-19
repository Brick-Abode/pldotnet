using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseNumberIntTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseNumberIntTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Numbers",
            Arguments = new List<FunctionArgument> { new FunctionArgument("count", "int4") },
            ReturnType = "SETOF int4",
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
            "c#-srf-sum",
            "numbers-1",
            @"WITH data AS (SELECT numbers() AS num LIMIT 100)",
            "SUM(num) = 4950"
        };

        yield return new object[]
        {
            "c#-srf-sum",
            "numbers-2",
            @"WITH data AS (SELECT numbers(100) AS num)",
            "SUM(num) = 4950"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestNumbers(
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
public class NumberIntTestsCsharp : BaseNumberIntTests
{
    protected override string FunctionBody =>
        @"
if(count == null){ for(int i=0;;i++) { yield return i; } }
	else { for(int i=0;i<count;i++) { yield return i; } }
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
