
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutArray10Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
    ArrayManipulation.FlatArray(values_array, ref flatten_values);
    nulls = 0;
    for(int i = 0; i < flatten_values.Length; i++)
    {
        if (flatten_values.GetValue(i) == null){
            nulls++;
            continue;
        }

        PhysicalAddress orig_value = (PhysicalAddress)flatten_values.GetValue(i);
        byte[] bytes = orig_value.GetAddressBytes();
        bytes[0] += 1;
        PhysicalAddress new_value = new PhysicalAddress(bytes);

        flatten_values.SetValue((PhysicalAddress)new_value, i);
    }
    values_array = flatten_values;
    ";

    public InOutArray10Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutArray10",
            Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT values_array", "MACADDR[]"), new FunctionArgument("OUT nulls", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
         {new object[]{
        "inout_array_10",
        "c#-inout-array-10",
        "ARRAY[MACADDR '08-00-2b-01-02-03',MACADDR '09-00-2b-01-02-03',null::macaddr,MACADDR 'a8-00-2b-01-02-03',null::macaddr]",
        "= ROW(ARRAY[MACADDR '09-00-2b-01-02-03',MACADDR '0a-00-2b-01-02-03',null::macaddr,MACADDR 'a9-00-2b-01-02-03',null::macaddr],2)"
        }};
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutArray10(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
