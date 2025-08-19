using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyNetMaskCidrTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyNetMaskCidrTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyNetMaskCidr", Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "CIDR"), new FunctionArgument("delta", "INT") }, ReturnType = "CIDR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-cidr", "modifyNetmask_CIDR1", "CIDR '2001:4f8:3:ba::/64', 10", "= CIDR '2001:4f8:3:ba::/74'" }, new object[] { "c#-cidr-null", "modifyNetmask_CIDR2", "NULL::CIDR, 10", "= CIDR '127.0.0.0/31'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyNetMaskCidr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ModifyNetMaskCidrTestsCSharp : BaseModifyNetMaskCidrTests
{
    protected override string FunctionBody => @"
if (my_inet == null)
    my_inet = (IPAddress.Parse(""127.0.0.0""), 21);

(IPAddress Address, int Netmask) originalInet = ((IPAddress Address, int Netmask))my_inet;
return (originalInet.Address, (int)(originalInet.Netmask + delta));
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}