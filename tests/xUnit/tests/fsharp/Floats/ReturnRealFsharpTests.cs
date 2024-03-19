
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class ReturnRealFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
1.50055f
    ";

    public ReturnRealFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnRealFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "real",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4", "returnRealFSharp", "", "= real '1.50055'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnRealFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
