
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class UpdateVarBitArrayIndexFsharpTests : PlDotNetTest
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

    public UpdateVarBitArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateVarBitArrayIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a BIT", "VARYING[]"), new FunctionArgument("b BIT", "VARYING") },
            ReturnType = "BIT VARYING[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varbit-null-1array", "updateVarbitArrayIndexFSharp1", "ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING], '1111111001111'::BIT VARYING", "= ARRAY['1111111001111'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING]" },
        new object[] { "f#-varbit-null-2array", "updateVarbitArrayIndexFSharp2", "ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING", "= ARRAY[['1111111001111'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]" },
        new object[] { "f#-varbit-null-3array", "updateVarbitArrayIndexFSharp3", "ARRAY[[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]], '1111111001111'::BIT VARYING", "= ARRAY[[['1111111001111'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]]" },
        new object[] { "f#-varbit-null-2array-arraynull", "updateVarbitArrayIndexFSharp4", "ARRAY[[null::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING", "= ARRAY[['1111111001111'::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateVarBitArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
