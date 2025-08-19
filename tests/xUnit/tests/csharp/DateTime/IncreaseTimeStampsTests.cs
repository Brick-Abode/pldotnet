using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseTimeStampsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseTimeStampsTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseTimeStamps", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIMESTAMP[]"), new FunctionArgument("days_to_add", "INT") }, ReturnType = "TIMESTAMP[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-timestamp-1array", "IncreaseTimestamps", "ARRAY[TIMESTAMP '2004-12-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], 2", "= ARRAY[TIMESTAMP '2004-12-21 10:23:54 PM', TIMESTAMP '2020-10-21 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-27 10:23:54 PM']" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimeStamps(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class IncreaseTimeStampsTestsCSharp : BaseIncreaseTimeStampsTests
{
    protected override string FunctionBody => @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    DateTime orig_value = (DateTime)flatten_values.GetValue(i);
    DateTime new_value = orig_value.AddDays((double)days_to_add);

    flatten_values.SetValue((DateTime)new_value, i);
}
return flatten_values;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}