
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class IdentityStrFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a
    ";

    public IdentityStrFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IdentityStrFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text") },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-text", "identityStrFSharp", "'dog'", "= 'dog'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIdentityStrFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
