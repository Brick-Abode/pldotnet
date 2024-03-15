
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutSimple10Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int i;

    // get bytes
    byte[] bytes = address.Address.GetAddressBytes();

    // compute checksum
    checksum = 0;
    for(i = 0; i<bytes.Length;i++){ checksum += bytes[i]; }
    ";

    public InOutSimple10Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutSimple10",
            Arguments = new List<FunctionArgument> { new FunctionArgument("OUT checksum", "INT"), new FunctionArgument("IN address", "INET") },
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
            new object[] { "c#-inout-simple-10", "inout_simple_10", "CIDR '192.168/24'", "= 360" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutSimple10(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
