
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class BooleanOrFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a.Value || b.Value
    ";

    public BooleanOrFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "BooleanOrFsharp",
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
            "BooleanOrFSharp1", 
            "false, false", 
            "is false" 
        },
        new object[] 
        { 
            "f#-bool-null", 
            "BooleanOrFSharp2", 
            "true, NULL::BOOLEAN", 
            "is true" 
        }
        };       
    }
    

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanOrFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
