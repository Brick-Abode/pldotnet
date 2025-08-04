using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseMinutesTimeArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseMinutesTimeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseMinutesTimeArray", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIME[]"), new FunctionArgument("min_to_add", "INT") }, ReturnType = "TIME[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-time-1array", "IncreaseMinutesTimeArray1", "ARRAY[TIME '05:30 PM', TIME '06:30 PM', null::time, TIME '09:30 AM'], 15", "= ARRAY[TIME '05:45 PM', TIME '06:45 PM', null::time, TIME '09:45 AM']" }, new object[] { "c#-time-2array", "IncreaseMinutesTimeArray", "ARRAY[[TIME '05:30 PM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']], 15", "= ARRAY[TIME '05:45 PM', TIME '06:45 PM', null::time, TIME '09:45 AM']" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMinutesTimeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class IncreaseMinutesTimeArrayTestsCSharp : BaseIncreaseMinutesTimeArrayTests
{
    protected override string FunctionBody => @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;


    TimeOnly orig_value = (TimeOnly)flatten_values.GetValue(i);
    TimeOnly new_value = orig_value.AddMinutes((double) min_to_add);

    flatten_values.SetValue((TimeOnly)new_value, i);
}
return flatten_values;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}