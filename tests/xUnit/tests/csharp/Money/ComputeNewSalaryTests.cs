
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class ComputeNewSalaryTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
decimal aux = (decimal)(1.0+rate);
    return (decimal)salary*aux;
    ";

    public ComputeNewSalaryTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ComputeNewSalary",
            Arguments = new List<FunctionArgument> { new FunctionArgument("salary", "MONEY"), new FunctionArgument("rate", "FLOAT8") },
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
            new object[] { "c#-money", "computeNewSalary", "'32500'::MONEY, 0.059875", "= '34445.9375'::MONEY" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestComputeNewSalary(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
