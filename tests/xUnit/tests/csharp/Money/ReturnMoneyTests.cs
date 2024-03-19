
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class ReturnMoneyTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
decimal s = salary == null ? 0.0M : (decimal)salary;
    decimal b = bonus == null ? 0.0M : (decimal)bonus;
    decimal d = discounts == null ? 0.0M : (decimal)discounts;
    return s+b-d;
    ";

    public ReturnMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnMoney",
            Arguments = new List<FunctionArgument> { new FunctionArgument("salary", "MONEY"), new FunctionArgument("bonus", "MONEY"), new FunctionArgument("discounts", "MONEY") },
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
            new object[] { "c#-money", "returnMoney1", "'32500.0'::MONEY, '1556.25'::MONEY, '899.99'::MONEY", "= '33156.26'::MONEY" },
        new object[] { "c#-money-null", "returnMoney2", "'13525.21'::MONEY, null::MONEY, '899.99'::MONEY", "= '12625.22'::MONEY" },
        new object[] { "c#-money-null", "returnMoney3", "null::MONEY, null::MONEY, null::MONEY", "= '0'::MONEY" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
