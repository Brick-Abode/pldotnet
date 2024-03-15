
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseInT8RangesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlRange<long> orig_value = (NpgsqlRange<long>)flatten_values.GetValue(i);
    NpgsqlRange<long> new_value = new NpgsqlRange<long>(orig_value.LowerBound + 1, orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound + 1, orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);

    flatten_values.SetValue((NpgsqlRange<long>)new_value, i);
}
return flatten_values;
    ";

    public IncreaseInT8RangesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT8Ranges",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INT8RANGE[]") },
            ReturnType = "INT8RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8range-null-1array", "IncreaseInt8Ranges1", "ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]", "= ARRAY['[3,7)'::INT8RANGE, '(,7)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT8Ranges(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
