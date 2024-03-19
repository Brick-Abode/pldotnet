
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class UpdateBitArrayIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
    ";

    public UpdateBitArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateBitArrayIndexFsharp",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("a", "BIT(8)[]"),
                new FunctionArgument("b", "BIT(8)"),
            },
            ReturnType = "BIT(8)[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bit-null-1array", "updateBitArrayIndexFSharp1", "ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)], '11111111'::BIT(8)", "= ARRAY['11111111'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)]" },
            new object[] { "f#-bit-null-2array", "updateBitArrayIndexFSharp2", "ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8)", "= ARRAY[['11111111'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]" },
            new object[] { "f#-bit-null-3array", "updateBitArrayIndexFSharp3", "ARRAY[[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]], '11111111'::BIT(8)", "= ARRAY[[['11111111'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]]" },
            new object[] { "f#-bit-null-2array-arraynull", "updateBitArrayIndexFSharp4", "ARRAY[[null::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8)", "= ARRAY[['11111111'::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateBitArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
