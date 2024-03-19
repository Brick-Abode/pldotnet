
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class CompareMacAddressFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
System.Object.ReferenceEquals(address1, address2)
    ";

    public CompareMacAddressFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CompareMacAddressFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("address1", "MACADDR"), new FunctionArgument("address2", "MACADDR") },
            ReturnType = "BOOLEAN",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
     new object[] 
        { 
            "f#-macaddr", 
            "compareMacAddressFSharp1", 
            "MACADDR '08:00:2a:01:02:03', MACADDR '08-00-2b-01-02-03'", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr-null", 
            "compareMacAddressFSharp2", 
            "NULL::MACADDR, MACADDR '08-00-2a-01-02-03'", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr-null", 
            "compareMacAddressFSharp3", 
            "MACADDR '08-00-2a-01-02-03', NULL::MACADDR", 
            "is false" 
        },
        new object[] 
        { 
            "f#-macaddr-null", 
            "compareMacAddressFSharp4", 
            "NULL::MACADDR, NULL::MACADDR", 
            "is true" 
        },        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompareMacAddressFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
