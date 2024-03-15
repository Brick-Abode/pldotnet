
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnRealTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return 1.50055f;
    ";

    public ReturnRealTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnReal",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "real",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float4", "returnReal", "", "= real '1.50055'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnReal(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
