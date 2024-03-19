
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class IncreaseInEtAddressTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    (IPAddress Address, int Netmask) orig_value = ((IPAddress Address, int Netmask))flatten_values.GetValue(i);
    byte[] bytes = orig_value.Address.GetAddressBytes();
    bytes[0] += 1;
    (IPAddress Address, int Netmask) new_value = (new IPAddress(bytes), orig_value.Netmask);

    flatten_values.SetValue(((IPAddress Address, int Netmask))new_value, i);
}
return flatten_values;
    ";

    public IncreaseInEtAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInEtAddress",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INET[]") },
            ReturnType = "INET[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inet-1array", "IncreaseInetAddress1", "ARRAY[INET '192.168.0.1/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24']", "= ARRAY[INET '193.168.0.1/24', INET '193.170.0.1/24', null::inet, INET '171.168.0.1/24']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInEtAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
