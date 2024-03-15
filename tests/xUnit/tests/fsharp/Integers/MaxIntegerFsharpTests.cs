
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxIntegerFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Nullable 2147483647
    ";

    public MaxIntegerFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MaxIntegerFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "int4",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4", "maxIntegerFSharp", "", "= int4 '2147483647'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxIntegerFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
