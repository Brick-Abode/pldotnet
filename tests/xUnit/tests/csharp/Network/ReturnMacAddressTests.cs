
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ReturnMacAddressTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return my_address;
    ";

    public ReturnMacAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnMacAddress",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR") },
            ReturnType = "MACADDR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr", "returnMacAddress", "MACADDR '08-00-2b-01-02-03'", "= MACADDR '08-00-2b-01-02-03'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMacAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
