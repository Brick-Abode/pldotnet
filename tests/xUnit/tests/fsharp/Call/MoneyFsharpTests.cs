
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Call")]
public class MoneyFSharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else 0
a
    ";

    public MoneyFSharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "testMoneyFSharp",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("a", "MONEY")
            },
            ReturnType = "MONEY",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-money", "testMoneyFSharp1", "'32500.0'::MONEY", " = '32500.0'::MONEY" },
            new object[] { "f#-money", "testMoneyFSharp1", "NULL::MONEY", " = 0::MONEY" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestNone(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
