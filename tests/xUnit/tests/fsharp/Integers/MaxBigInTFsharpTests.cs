
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxBigInTFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Nullable 9223372036854775807L
    ";

    public MaxBigInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MaxBigInTFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "int8",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8", "maxBigIntFSharp", "", "= int8 '9223372036854775807'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxBigInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
