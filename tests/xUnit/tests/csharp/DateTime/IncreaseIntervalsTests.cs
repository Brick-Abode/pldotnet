
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class IncreaseIntervalsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlInterval orig_value = (NpgsqlInterval)flatten_values.GetValue(i);
    NpgsqlInterval new_value = new NpgsqlInterval(orig_value.Months + months_to_add, orig_value.Days + days_to_add, orig_value.Time);

    flatten_values.SetValue((NpgsqlInterval)new_value, i);
}
return flatten_values;
    ";

    public IncreaseIntervalsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseIntervals",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INTERVAL[]"), new FunctionArgument("months_to_add", "INT"), new FunctionArgument("days_to_add", "INT") },
            ReturnType = "INTERVAL[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-interval-1array", "IncreaseIntervals1", "ARRAY[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], 2, 5", "= ARRAY[INTERVAL '2 mons 5 days 4 hours 5 minutes 6 seconds', INTERVAL '2 mons 5 days 8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 10 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds']" },
        new object[] { "c#-interval-2array", "IncreaseIntervals2", "ARRAY[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], 2, 5", "= ARRAY[INTERVAL '2 mons 5 days 4 hours 5 minutes 6 seconds', INTERVAL '2 mons 5 days 8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 10 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseIntervals(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
