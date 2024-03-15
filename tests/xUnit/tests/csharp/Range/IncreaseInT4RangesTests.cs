
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseInT4RangesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlRange<int> orig_value = (NpgsqlRange<int>)flatten_values.GetValue(i);
    NpgsqlRange<int> new_value = new NpgsqlRange<int>(orig_value.LowerBound + 1, orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound + 1, orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);

    flatten_values.SetValue((NpgsqlRange<int>)new_value, i);
}
return flatten_values;
    ";

    public IncreaseInT4RangesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT4Ranges",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INT4RANGE[]") },
            ReturnType = "INT4RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4range-null-1array", "IncreaseInt4Ranges1", "ARRAY['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE]", "= ARRAY['[3,7)'::INT4RANGE, '(,7)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT4Ranges(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
