using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseMakePiTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseMakePiTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "make_pi",
            Arguments = new List<FunctionArgument>(),
            ReturnType = "SETOF float8",
            Language = Language,
            IsStrict = false,
            Body = FunctionBody,
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
            "c#-srf-pi",
            "make_pi-1",
            @"
WITH data AS (SELECT numbers() AS num, make_pi() AS pi_value)",
            "pi_value < 3.143",
            "WHERE num = 1000 LIMIT 1"
        };

        yield return new object[]
        {
            "c#-srf-pi",
            "make_pi-2",
            @"
WITH data AS (SELECT numbers() AS num, make_pi() AS pi_value)",
            "pi_value > 3.141",
            "WHERE num = 1000 LIMIT 1"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMakePi(
        string featureName,
        string testName,
        string cteStatement,
        string customAssertion,
        string querySuffix
    )
    {
        SetupTest(cteStatement);
        RunTestWithSuffix(featureName, testName, this.cteStatement, customAssertion, querySuffix);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SRF")]
public class MakePiTestsCsharp : BaseMakePiTests
{
    protected override string FunctionBody =>
        @"double sum = 0.0;
        for (int i = 0; ; i++) { yield return 4 * (sum += ((i % 2) == 0 ? 1.0 : -1.0) / (2 * i + 1)); }";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
