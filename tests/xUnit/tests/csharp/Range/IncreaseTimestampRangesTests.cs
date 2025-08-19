using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseTimestampRangesTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseTimestampRangesTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseTimestampRanges", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TSRANGE[]") }, ReturnType = "TSRANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-tsrange-null-1array", "IncreaseTimestampRanges1", "ARRAY['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE]", "= ARRAY['[2021-01-02 14:30, 2021-01-02 15:30)'::TSRANGE, '(, 2021-04-02 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampRanges(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampRangesTestsCSharp : BaseIncreaseTimestampRangesTests
{
    protected override string FunctionBody => @"
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
    protected override LanguageType Language => LanguageType.PlcSharp;
}