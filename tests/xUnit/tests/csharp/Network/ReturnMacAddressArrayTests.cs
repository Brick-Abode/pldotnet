using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnMacAddressArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnMacAddressArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnMacAddressArray", Arguments = new List<FunctionArgument> { new FunctionArgument("addresses", "MACADDR[]") }, ReturnType = "MACADDR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr-null-2array-arraynull", "returnMacAddressArray1", "ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]", "= ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddressArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressArrayTestsCSharp : BaseReturnMacAddressArrayTests
{
    protected override string FunctionBody => @"
return addresses;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}