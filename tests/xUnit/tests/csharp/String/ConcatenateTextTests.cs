
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ConcatenateTextTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
        a = """";

    if (b == null)
        b = """";

    string c = a + "" "" + b;
    return c;
    ";

    public ConcatenateTextTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateText",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "text") },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-text", "concatenateText1", "'red', 'blue'", "= 'red blue'" },
        new object[] { "c#-text-null", "concatenateText2", "NULL::TEXT, 'blue'", "= ' blue'" },
        new object[] { "c#-text", "concatenateText3", "'КРАСНЫЙ', 'СИНИЙ'", "= 'КРАСНЫЙ СИНИЙ'::TEXT" },
        new object[] { "c#-text", "concatenateText4", "'赤', '青い'", "= '赤 青い'::TEXT" },
        new object[] { "c#-text", "concatenateText5", "'紅色的', '藍色的'", "= '紅色的 藍色的'::TEXT" },
        new object[] { "c#-text", "concatenateText6", "'🐂', '🥰'", "= '🐂 🥰'::TEXT" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateText(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
