
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class ReturnMinMoneyTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
decimal value = -92233720368547758.08M;
    return value;
    ";

    public ReturnMinMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnMinMoney",
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
            new object[] { "c#-money", "returnMinMoney", "", "= '-92233720368547758.08'::MONEY" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMinMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
