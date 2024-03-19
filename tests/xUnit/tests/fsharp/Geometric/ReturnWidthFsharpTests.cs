
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class ReturnWidthFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let new_box = NpgsqlBox(high, low)
    Math.Abs(new_box.Width)
    ";

    public ReturnWidthFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnWidthFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") },
            ReturnType = "float8",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-box", "returnWidthFSharp", "POINT '(0.025988, 1.021653)', POINT '(2.052787, 3.005716)'", "= float8 '2.026799'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnWidthFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
