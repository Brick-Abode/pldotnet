
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class ToggleFirstBitsFsharpTests : PlDotNetTest
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

    public ToggleFirstBitsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ToggleFirstBitsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BIT(8)[]") },
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
            new object[] { "f#-bit-null-1array", "ToggleFirstBitsFSharp1", "ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '01101001'::BIT(8)]", "= ARRAY['00101001'::BIT(8), '00101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestToggleFirstBitsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
