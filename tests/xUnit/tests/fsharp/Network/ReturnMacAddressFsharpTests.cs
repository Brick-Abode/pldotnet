using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnMacAddressFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnMacAddressFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnMacAddressFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR") }, ReturnType = "MACADDR", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-macaddr", "returnMacAddressFSharp", "MACADDR '08-00-2b-01-02-03'", "= MACADDR '08-00-2b-01-02-03'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddressFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressFsharpTestsFSharp : BaseReturnMacAddressFsharpTests
{
    protected override string FunctionBody => @"
my_address
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}