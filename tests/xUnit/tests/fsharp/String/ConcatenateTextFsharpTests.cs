
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ConcatenateTextFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a + "" "" + b
    ";

    public ConcatenateTextFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateTextFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "text") },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-text", "concatenateTextFSharp1", "'red', 'blue'", "= 'red blue'" },
        new object[] { "f#-text-null", "concatenateTextFSharp2", "NULL::TEXT, 'blue'", "= ' blue'" },
        new object[] { "f#-text-null", "concatenateTextFSharp3", "NULL::TEXT, NULL::TEXT", "= ' '" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateTextFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
