
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
addresses
    ";

    public ReturnMacAddressArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnMacAddressArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("addresses", "MACADDR[]") },
            ReturnType = "MACADDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-macaddr-null-2array-arraynull", "returnMacAddressArrayFSharp1", "ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]", "= ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddressArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
