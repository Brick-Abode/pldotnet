
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class AddOneMacAddress8FsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let bytes = my_address.GetAddressBytes()
bytes[7] <- bytes[7] + byte 1
PhysicalAddress(bytes)
    ";

    public AddOneMacAddress8FsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddOneMacAddress8Fsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_address", "MACADDR8") },
            ReturnType = "MACADDR8",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-macaddr8", "addOneMacAddress8FSharp", "MACADDR8 '08:00:2b:01:02:03:04:05'", "= MACADDR8 '08:00:2b:01:02:03:04:06'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddOneMacAddress8Fsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
