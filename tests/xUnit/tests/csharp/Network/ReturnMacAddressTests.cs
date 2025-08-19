using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnMacAddressTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnMacAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnMacAddress", Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR") }, ReturnType = "MACADDR", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr", "returnMacAddress", "MACADDR '08-00-2b-01-02-03'", "= MACADDR '08-00-2b-01-02-03'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressTestsCSharp : BaseReturnMacAddressTests
{
    protected override string FunctionBody => @"
return my_address;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}