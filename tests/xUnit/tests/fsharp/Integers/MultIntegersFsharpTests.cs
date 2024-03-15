
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MultIntegersFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Nullable (a*b)
    ";

    public MultIntegersFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MultIntegersFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int4"), new FunctionArgument("b", "int4") },
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
            new object[] { "f#-int4", "multIntegersFSharp", "15, 15", "= int4 '225'" },
        new object[] { "f#-int4", "multIntegersFSharp", "50, 75", "= int4 '3750'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultIntegersFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
