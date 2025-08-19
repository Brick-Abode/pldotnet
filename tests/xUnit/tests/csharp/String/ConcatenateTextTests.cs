using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateTextTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateTextTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateText", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "text") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text", "concatenateText1", "'red', 'blue'", "= 'red blue'" }, new object[] { "c#-text-null", "concatenateText2", "NULL::TEXT, 'blue'", "= ' blue'" }, new object[] { "c#-text", "concatenateText3", "'КРАСНЫЙ', 'СИНИЙ'", "= 'КРАСНЫЙ СИНИЙ'::TEXT" }, new object[] { "c#-text", "concatenateText4", "'赤', '青い'", "= '赤 青い'::TEXT" }, new object[] { "c#-text", "concatenateText5", "'紅色的', '藍色的'", "= '紅色的 藍色的'::TEXT" }, new object[] { "c#-text", "concatenateText6", "'🐂', '🥰'", "= '🐂 🥰'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateText(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ConcatenateTextTestsCSharp : BaseConcatenateTextTests
{
    protected override string FunctionBody => @"
if (a == null)
        a = """";

    if (b == null)
        b = """";

    string c = a + "" "" + b;
    return c;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}