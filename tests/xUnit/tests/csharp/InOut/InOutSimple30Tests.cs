
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutSimple30Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int i;

    // compute new address
    (IPAddress Address, int Netmask) address2 = address ?? (IPAddress.Parse(""1.1.1.1""), 8);
    byte[] bytes = address2.Address.GetAddressBytes();
    bytes[pos]+=(byte)delta;
    address = (new IPAddress(bytes), address2.Netmask);

    // compute checksum
    checksum = 0;
    for(i = 0; i<bytes.Length;i++){ checksum += bytes[i]; }
    ";

    public InOutSimple30Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutSimple30",
            Arguments = new List<FunctionArgument> { new FunctionArgument("OUT checksum", "INT"), new FunctionArgument("INOUT address", "INET"), new FunctionArgument("IN pos", "INT"), new FunctionArgument("IN delta", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inout-simple-30", "inout_simple_30", "CIDR '192.168/24', 2, 3", "= ROW(363, INET '192.168.3.0/24')" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutSimple30(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
