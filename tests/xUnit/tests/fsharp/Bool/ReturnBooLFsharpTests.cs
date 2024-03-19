
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class ReturnBooLFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
false
    ";

    public ReturnBooLFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnBooLFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
    new object[] 
        { 
            "f#-bool", 
            "returnBoolFSharp", 
            "", 
            "is false" 
        }        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBooLFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
