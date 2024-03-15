
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CompareMacAddressTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (address1 == null)
    address1 = new PhysicalAddress(new byte[6] {171, 1, 43, 49, 65, 250});

if (address2 == null)
    address2 = new PhysicalAddress(new byte[6] {171, 1, 43, 49, 65, 250});

return ((PhysicalAddress)address1).Equals(((PhysicalAddress)address2));
    ";

    public CompareMacAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CompareMacAddress",
            Arguments = new List<FunctionArgument> { new FunctionArgument("address1", "MACADDR"), new FunctionArgument("address2", "MACADDR") },
            ReturnType = "BOOLEAN",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

     public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr", "compareMacAddress1", "MACADDR '08:00:2a:01:02:03', MACADDR '08-00-2b-01-02-03'", "is false" },
            new object[] { "c#-macaddr-null", "compareMacAddress2", "NULL::MACADDR, MACADDR '08-00-2a-01-02-03'", "is false" }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompareMacAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
