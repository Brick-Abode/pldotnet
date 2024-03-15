
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ModifyIpCidrTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
byte[] bytes = my_inet.Address.GetAddressBytes();
bytes[pos]+=(byte)delta;
return (new IPAddress(bytes), my_inet.Netmask);
    ";

    public ModifyIpCidrTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyIpCidr",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "CIDR"), new FunctionArgument("pos", "INT"), new FunctionArgument("delta", "INT") },
            ReturnType = "CIDR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-cidr", "modifyIP_CIDR", "CIDR '192.168/24', 0, 6", "= CIDR '198.168.0.0/24'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIpCidr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
