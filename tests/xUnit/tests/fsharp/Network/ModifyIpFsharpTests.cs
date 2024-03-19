
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class ModifyIpFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let struct (address, netmask) = if my_inet.HasValue then my_inet.Value else (IPAddress.Parse(""127.0.0.1""), 21)
let bytes = address.GetAddressBytes()
let size = bytes.Length
bytes[size-1] <- bytes[size-1] + byte n.Value
struct (IPAddress(bytes), netmask)
    ";

    public ModifyIpFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyIpFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_inet", "INET"), new FunctionArgument("n", "INT") },
            ReturnType = "INET",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-inet", "modifyIPFSharp1", "INET '2001:db8:3333:4444:5555:6666:1.2.3.4/25', 20", "= INET '2001:db8:3333:4444:5555:6666:1.2.3.24/25'" },
        new object[] { "f#-inet-null", "modifyIPFSharp2", "NULL::INET, 20", "= INET '127.0.0.21/21'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIpFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
