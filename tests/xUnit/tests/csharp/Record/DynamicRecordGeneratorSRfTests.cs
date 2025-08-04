using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseDynamicRecordGeneratorSRfTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseDynamicRecordGeneratorSRfTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "DynamicRecordGeneratorSRf",
            Arguments = new List<FunctionArgument> { new FunctionArgument("lim", "INT8") },
            ReturnType = "SETOF record",
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
            "c#-drec-srf-sum",
            "drec-srf-1a",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorSRf(10)
        AS t(a int8, b text)
)",
            "SUM(a) = 45"
        };

        yield return new object[]
        {
            "c#-drec-srf-sum",
            "drec-srf-2b",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorSRf(10)
        AS t(a int8, b text)
        WHERE a = 5
)",
            "b = 'Number is 5'"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDynamicRecordGeneratorSRfTests(
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
[Trait("Category", "Record")]
public class DynamicRecordGeneratorSRfTestsCsharp : BaseDynamicRecordGeneratorSRfTests
{
    protected override string FunctionBody =>
        @"
if (!(lim > 0)){ yield break; }
    for(long i=0;i<lim;i++){ yield return new object?[] { (long)i, $""Number is {i}"" }; }
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}
