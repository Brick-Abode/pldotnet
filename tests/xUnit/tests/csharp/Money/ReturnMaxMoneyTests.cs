
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class ReturnMaxMoneyTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
decimal value = 92233720368547758.07M;
    return value;
    ";

    public ReturnMaxMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnMaxMoney",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "MONEY",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-money", "returnMaxMoney", "", "= '92233720368547758.07'::MONEY" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMaxMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
