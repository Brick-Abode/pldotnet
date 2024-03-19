
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseDateOnlyRangesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlRange<DateOnly> orig_value = (NpgsqlRange<DateOnly>)flatten_values.GetValue(i);
    NpgsqlRange<DateOnly> new_value = new NpgsqlRange<DateOnly>(orig_value.LowerBound.AddDays(1), orig_value.LowerBoundIsInclusive, orig_value.LowerBoundInfinite, orig_value.UpperBound.AddDays(1), orig_value.UpperBoundIsInclusive, orig_value.UpperBoundInfinite);

    flatten_values.SetValue((NpgsqlRange<DateOnly>)new_value, i);
}
return flatten_values;
    ";

    public IncreaseDateOnlyRangesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseDateOnlyRanges",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "DATERANGE[]") },
            ReturnType = "DATERANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-daterange-null-1array", "IncreaseDateonlyRanges1", "ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-04)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]", "= ARRAY['[2021-01-02, 2021-01-02)'::DATERANGE, '(, 2021-04-05)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseDateOnlyRanges(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
