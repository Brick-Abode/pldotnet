
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class ToggleFirstVarBitsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    for i in 0 .. values_array.Length - 1 do
        match values_array.GetValue(i) with
        | :? BitArray as orig_value ->
            let new_value = new BitArray(orig_value)
            new_value.[0] <- not new_value.[0]
            values_array.SetValue(new_value, i)
        | _ -> ()
    values_array
    ";

    public ToggleFirstVarBitsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ToggleFirstVarBitsFsharp",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("values_array BIT", "VARYING[]")
            },
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
            new object[] {
                "f#-varbit-null-1array",
                "ToggleFirstVarbitsFSharp1",
                "ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '001001'::BIT VARYING]",
                "= ARRAY['0010101101101'::BIT VARYING, '001011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING]"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestToggleFirstVarBitsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
