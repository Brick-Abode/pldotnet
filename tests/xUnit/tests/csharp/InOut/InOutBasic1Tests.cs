
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutBasic1Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
argument_0 = 1;
    ";

    public InOutBasic1Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutBasic1",
            Arguments = new List<FunctionArgument> { new FunctionArgument("OUT argument_0", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inout-basic-1", "inout_basic_1", "", "= 1" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutBasic1(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
