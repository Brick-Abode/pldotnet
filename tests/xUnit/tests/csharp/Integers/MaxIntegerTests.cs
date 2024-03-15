
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MaxIntegerTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return 2147483647;
    ";

    public MaxIntegerTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MaxInteger",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "integer",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4", "maxInteger", "", "= integer '2147483647'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxInteger(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
