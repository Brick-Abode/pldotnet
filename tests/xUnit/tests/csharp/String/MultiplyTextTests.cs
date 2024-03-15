
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class MultiplyTextTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int i;
    string c = """";
    for(i=0;i<b;i++){ c = c + a; }
    return c;
    ";

    public MultiplyTextTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MultiplyText",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "int") },
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
            new object[] { "c#-text", "multiplyText", "'dog ', 3", "= 'dog dog dog '" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyText(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
