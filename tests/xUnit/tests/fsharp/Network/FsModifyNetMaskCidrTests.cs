using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseFsModifyNetMaskCidrTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseFsModifyNetMaskCidrTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyNetMaskCidr", Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "CIDR"), new FunctionArgument("delta", "INT") }, ReturnType = "CIDR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-cidr", "modifyNetmask_CIDR1", "CIDR '2001:4f8:3:ba::/64', 10", "= CIDR '2001:4f8:3:ba::/74'" }, new object[] { "f#-cidr-null", "modifyNetmask_CIDR2", "NULL::CIDR, 10", "= CIDR '127.0.0.0/31'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyNetMaskCidr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class FsModifyNetMaskCidrTestsFSharp : BaseFsModifyNetMaskCidrTests
{
    protected override string FunctionBody => @"
let struct (address, netmask) = if my_inet.HasValue then my_inet.Value else (IPAddress.Parse(""127.0.0.0""), 21)
struct (address, int (netmask + delta.Value))
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}