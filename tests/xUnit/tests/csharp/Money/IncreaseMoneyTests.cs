
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class IncreaseMoneyTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    decimal orig_value = (decimal)flatten_values.GetValue(i);
    decimal new_value = orig_value + 1;

    flatten_values.SetValue((decimal)new_value, i);
}
return flatten_values;
    ";

    public IncreaseMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMoney",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MONEY[]") },
            ReturnType = "MONEY[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-money-null-1array", "IncreaseMoney1", "ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY]", "= ARRAY['32501.0'::MONEY, '-499.4'::MONEY, null::MONEY, '900541.2'::MONEY]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
