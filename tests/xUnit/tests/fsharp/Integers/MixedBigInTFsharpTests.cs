
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MixedBigInTFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Nullable ((int64)a + (int64)b + c)
    ";

    public MixedBigInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MixedBigInTFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int2"), new FunctionArgument("b", "int4"), new FunctionArgument("c", "int8") },
            ReturnType = "int8",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8", "mixedBigIntFSharp", "'32767'::int2,  '2147483647'::int4, '100'::int8", "= int8 '2147516514'" },
        new object[] { "f#-int8", "mixedBigIntFSharp", "'32767'::int2,  '2147483647'::int4, '2147483647'::int8", "= int8 '4295000061'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
