
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MixedBigInT2FsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (a.HasValue, b.HasValue) with
| (false, false) -> System.Nullable(int64 0)
| (true, false) -> Nullable(int64 a.Value)
| (false, true) -> Nullable(b.Value)
| (true, true) -> Nullable (int64 a.Value + b.Value)
    ";

    public MixedBigInT2FsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MixedBigInT2Fsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int2"), new FunctionArgument("b", "int8") },
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
            new object[] { "f#-int8", "mixedBigInt2FSharp1", "'32767'::int2, '2147483647'::int8", "= int8 '2147516414'" },
        new object[] { "f#-int8-null", "mixedBigInt2FSharp2", "'32767'::int2, NULL::int8", "= int8 '32767'" },
        new object[] { "f#-int8-null", "mixedBigInt2FSharp3", "NULL::int2, '2147483647'::int8", "= int8 '2147483647'" },
        new object[] { "f#-int8-null", "mixedBigInt2FSharp4", "NULL::int2, NULL::int8", "= int8 '0'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInT2Fsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
