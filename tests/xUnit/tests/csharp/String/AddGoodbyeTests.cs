
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class AddGoodbyeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (a + "" Goodbye ^.^"");
    ";

    public AddGoodbyeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddGoodbye",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR") },
            ReturnType = "BPCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bpchar", "testingBpChar", "'HELLO!'", "= 'HELLO! Goodbye ^.^'::BPCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddGoodbye(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
