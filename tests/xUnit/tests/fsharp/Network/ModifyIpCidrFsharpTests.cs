
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ModifyIpCidrFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let struct (address, netmask) = my_inet
let bytes = address.GetAddressBytes()
bytes[pos] <- bytes[pos] + byte delta
struct (IPAddress(bytes), netmask)
    ";

    public ModifyIpCidrFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyIpCidrFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "CIDR"), new FunctionArgument("pos", "INT"), new FunctionArgument("delta", "INT") },
            ReturnType = "CIDR",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-cidr", "modifyIP_CIDRFSharp", "CIDR '192.168/24', 0, 6", "= CIDR '198.168.0.0/24'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIpCidrFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
