
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class IncreaseMacAddressTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    PhysicalAddress orig_value = (PhysicalAddress)flatten_values.GetValue(i);
    byte[] bytes = orig_value.GetAddressBytes();
    bytes[0] += 1;
    PhysicalAddress new_value = new PhysicalAddress(bytes);

    flatten_values.SetValue((PhysicalAddress)new_value, i);
}
return flatten_values;
    ";

    public IncreaseMacAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMacAddress",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR[]") },
            ReturnType = "MACADDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr-1array", "IncreaseMacAddress1", "ARRAY[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03']", "= ARRAY[MACADDR '09-00-2b-01-02-03', MACADDR '0a-00-2b-01-02-03', null::macaddr, MACADDR 'a9-00-2b-01-02-03']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMacAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
