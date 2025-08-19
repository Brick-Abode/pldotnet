using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnMacAddressArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnMacAddressArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnMacAddressArrayFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("addresses", "MACADDR[]") }, ReturnType = "MACADDR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-macaddr-null-2array-arraynull", "returnMacAddressArrayFSharp1", "ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]", "= ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddressArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressArrayFsharpTestsFSharp : BaseReturnMacAddressArrayFsharpTests
{
    protected override string FunctionBody => @"
addresses
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}