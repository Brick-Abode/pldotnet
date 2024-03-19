
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class ToggleFirstBitsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    BitArray orig_value = (BitArray)flatten_values.GetValue(i);
    BitArray new_value = orig_value;
    new_value[0] = new_value[0] ? false : true;

    flatten_values.SetValue((BitArray)new_value, i);
}
return flatten_values;
    ";

    public ToggleFirstBitsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ToggleFirstBits",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BIT(8)[]") },
            ReturnType = "BIT(8)[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bit-null-1array", "ToggleFirstBits1", "ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '01101001'::BIT(8)]", "= ARRAY['00101001'::BIT(8), '00101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestToggleFirstBits(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
