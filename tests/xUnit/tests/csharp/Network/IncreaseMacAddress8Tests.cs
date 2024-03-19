
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class IncreaseMacAddress8Tests : PlDotNetTest
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

    public IncreaseMacAddress8Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMacAddress8",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR8[]") },
            ReturnType = "MACADDR8[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr8-1array", "IncreaseMacAddress81", "ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a8-00-2b-01-02-03-ab-ac']", "= ARRAY[MACADDR8 '09-00-2b-01-02-03-ab-ac', MACADDR8 '0a-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a9-00-2b-01-02-03-ab-ac']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMacAddress8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
