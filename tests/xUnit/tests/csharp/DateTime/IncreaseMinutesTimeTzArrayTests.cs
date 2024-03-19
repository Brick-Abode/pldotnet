
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class IncreaseMinutesTimeTzArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    DateTimeOffset orig_value = (DateTimeOffset)flatten_values.GetValue(i);
    DateTimeOffset new_value = orig_value.AddMinutes((double) min_to_add);

    flatten_values.SetValue((DateTimeOffset)new_value, i);
}
return flatten_values;
    ";

    public IncreaseMinutesTimeTzArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMinutesTimeTzArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIMETZ[]"), new FunctionArgument("min_to_add", "INT") },
            ReturnType = "TIMETZ[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timetz-1array", "IncreaseMinutesTimetzArray1", "ARRAY[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00'], 15", "= ARRAY[TIMETZ '05:45-03:00', TIMETZ '06:45-03:00', null::timetz, TIMETZ '22:45-03:00']" },
        new object[] { "c#-timetz-2array", "IncreaseMinutesTimetzArray2", "ARRAY[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']], 15", "= ARRAY[TIMETZ '05:45-03:00', TIMETZ '06:45-03:00', null::timetz, TIMETZ '22:45-03:00']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMinutesTimeTzArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
