
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ModifyNetMaskCidrTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (my_inet == null)
    my_inet = (IPAddress.Parse(""127.0.0.0""), 21);

(IPAddress Address, int Netmask) originalInet = ((IPAddress Address, int Netmask))my_inet;
return (originalInet.Address, (int)(originalInet.Netmask + delta));
    ";

    public ModifyNetMaskCidrTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyNetMaskCidr",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "CIDR"), new FunctionArgument("delta", "INT") },
            ReturnType = "CIDR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-cidr", "modifyNetmask_CIDR1", "CIDR '2001:4f8:3:ba::/64', 10", "= CIDR '2001:4f8:3:ba::/74'" },
        new object[] { "c#-cidr-null", "modifyNetmask_CIDR2", "NULL::CIDR, 10", "= CIDR '127.0.0.0/31'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyNetMaskCidr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
