
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class AddOneMacAddress8Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
byte[] bytes = my_address.GetAddressBytes();
bytes[7] += 1;
return new PhysicalAddress(bytes);
    ";

    public AddOneMacAddress8Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddOneMacAddress8",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR8") },
            ReturnType = "MACADDR8",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr8", "addOneMacAddress8", "MACADDR8 '08:00:2b:01:02:03:04:05'", "= MACADDR8 '08:00:2b:01:02:03:04:06'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddOneMacAddress8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
