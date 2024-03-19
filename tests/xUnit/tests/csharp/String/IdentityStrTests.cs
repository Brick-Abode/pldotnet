
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class IdentityStrTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
System.Console.WriteLine(""Got string: {0}"", a);
    return a;
    ";

    public IdentityStrTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IdentityStr",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text") },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-text", "identityStr", "'dog'", "= 'dog'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIdentityStr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
