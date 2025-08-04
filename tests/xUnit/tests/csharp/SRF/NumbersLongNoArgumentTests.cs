using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseNumbersLongNoArgumentTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseNumbersLongNoArgumentTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "NumbersLongNoArgument",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "SETOF int8",
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
            @"WITH data AS (SELECT NumbersLongNoArgument() AS num LIMIT 100)",
            "SUM(num) = 4950"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestNumbersLongNoArgument(
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
public class NumbersLongNoArgumentTestsCsharp : BaseNumbersLongNoArgumentTests
{
    protected override string FunctionBody =>
        @"
for(long i=0;;i++){ yield return i;}
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}
