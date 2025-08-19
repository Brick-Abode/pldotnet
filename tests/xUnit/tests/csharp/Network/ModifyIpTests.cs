using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyIpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyIpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyIp", Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "INET"), new FunctionArgument("n", "INT") }, ReturnType = "INET", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inet", "modifyNetMask1", "INET '2001:db8:3333:4444:5555:6666:1.2.3.4/25', 20", "= INET '2001:db8:3333:4444:5555:6666:1.2.3.24/25'" }, new object[] { "c#-inet-null", "modifyNetMask2", "NULL::INET, 20", "= INET '127.0.0.21/21'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ModifyIpTestsCSharp : BaseModifyIpTests
{
    protected override string FunctionBody => @"
if (my_inet == null)
    my_inet = (IPAddress.Parse(""127.0.0.1""), 21);

byte[] bytes = (((IPAddress Address, int Netmask))my_inet).Address.GetAddressBytes();
int size = bytes.Length;
bytes[size-1]+=(byte)n;
return (new IPAddress(bytes), (((IPAddress Address, int Netmask))my_inet).Netmask);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}