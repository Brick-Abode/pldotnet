
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class BooleanAndFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a else false
let b = if b.HasValue then b else false
a.Value && b.Value
    ";

    public BooleanAndFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "BooleanAndFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
        new object[] 
        { 
            "f#-bool", 
            "BooleanAndFSharp1", 
            "true, true", 
            "is true" 
        },
        new object[] 
        { 
            "f#-bool-null", 
            "BooleanAndFSharp2", 
            "NULL::BOOLEAN, true", 
            "is false" 
        }        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanAndFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
