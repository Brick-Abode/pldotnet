
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxSmallInTFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Nullable (32767s)
    ";

    public MaxSmallInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MaxSmallInTFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "int2",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int2", "maxSmallIntFSharp", "", "= int2 '32767'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxSmallInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
