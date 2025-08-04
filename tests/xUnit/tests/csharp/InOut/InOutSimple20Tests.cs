using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutSimple20Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutSimple20Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutSimple20", Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT address", "INET"), new FunctionArgument("IN pos", "INT"), new FunctionArgument("IN delta", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-simple-20", "inout_simple_20", "CIDR '192.168/24', 2, 3", "= INET '192.168.3.0/24'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutSimple20(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutSimple20TestsCSharp : BaseInOutSimple20Tests
{
    protected override string FunctionBody => @"
int i;

    // compute new address
    (IPAddress Address, int Netmask) address2 = address ?? (IPAddress.Parse(""1.1.1.1""), 8);
    byte[] bytes = address2.Address.GetAddressBytes();
    bytes[pos]+=(byte)delta;
    address = (new IPAddress(bytes), address2.Netmask);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}