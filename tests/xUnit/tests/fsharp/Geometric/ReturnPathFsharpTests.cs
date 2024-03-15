
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class ReturnPathFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
orig_path
    ";

    public ReturnPathFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnPathFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_path", "PATH") },
            ReturnType = "PATH",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-path",
                "returnPathFSharp - open",
                "PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'",
                "<= PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'"
            },
            new object[] {
                "f#-path",
                "returnPathFSharp - close",
                "PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'",
                "<= PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnPathFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
