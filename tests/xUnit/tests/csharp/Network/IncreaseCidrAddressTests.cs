
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class IncreaseCidrAddressTests : PlDotNetTest
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

    public IncreaseCidrAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseCidrAddress",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "CIDR[]") },
            ReturnType = "CIDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-cidr-1array", "IncreaseCIDRAddress1", "ARRAY[CIDR '192.168.231.0/24', CIDR '175.170.14.0/24', null::cidr, CIDR '167.168.41.0/24']", "= ARRAY[CIDR '193.168.231.0/24', CIDR '176.170.14.0/24', null::cidr, CIDR '168.168.41.0/24']" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseCidrAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
