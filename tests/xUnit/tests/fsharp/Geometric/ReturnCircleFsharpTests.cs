
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class ReturnCircleFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
orig_circle
    ";

    public ReturnCircleFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnCircleFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_circle", "CIRCLE") },
            ReturnType = "CIRCLE",
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
                "f#-circle",
                "returnCircleFSharp",
                "CIRCLE '2.5, 3.5, 12.78'",
                "~= CIRCLE '<(2.5, 3.5), 12.78>'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnCircleFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
