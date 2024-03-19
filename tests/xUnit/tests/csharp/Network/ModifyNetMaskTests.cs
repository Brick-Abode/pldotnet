
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class ModifyNetMaskTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (my_inet.Address, my_inet.Netmask + n);
    ";

    public ModifyNetMaskTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyNetMask",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "INET"), new FunctionArgument("n", "INT") },
            ReturnType = "INET",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inet", "modifyNetMask", "INET '192.168.0.1/24', 6", "= INET '192.168.0.1/30'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyNetMask(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
