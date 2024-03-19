
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class TestBoxFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
my_box
    ";

    public TestBoxFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "TestBoxFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_box", "BOX") },
            ReturnType = "BOX",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-box", "testBoxFSharp", "BOX '(0.025988, 1.021653), (2.052787, 3.005716)'", "= BOX '(0.025988, 1.021653), (2.052787, 3.005716)'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTestBoxFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
