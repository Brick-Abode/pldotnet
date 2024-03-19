
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class AddGoodbyeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a + "" Goodbye ^.^""
    ";

    public AddGoodbyeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddGoodbyeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR") },
            ReturnType = "BPCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bpchar", "testingBpCharFSharp", "'HELLO!'", "= 'HELLO! Goodbye ^.^'::BPCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddGoodbyeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
