
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampTzRangesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlRange<DateTime> orig_value = (NpgsqlRange<DateTime>)flatten_values.GetValue(i);
    NpgsqlRange<DateTime> new_value = new NpgsqlRange<DateTime>(orig_value.LowerBound.AddDays(1), orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound.AddDays(1), orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);

    flatten_values.SetValue((NpgsqlRange<DateTime>)new_value, i);
}
return flatten_values;
    ";

    public IncreaseTimestampTzRangesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseTimestampTzRanges",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TSTZRANGE[]") },
            ReturnType = "TSTZRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-tstzrange-null-1array", "IncreaseTimestampTzRanges1", "ARRAY['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE]", "= ARRAY['[2021-01-02 14:30 +02, 2021-01-02 15:30 -05)'::TSTZRANGE, '(, 2021-04-02 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampTzRanges(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
