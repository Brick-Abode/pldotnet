using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMultiplyTextTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMultiplyTextTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MultiplyText", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "int") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text", "multiplyText", "'dog ', 3", "= 'dog dog dog '" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyText(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class MultiplyTextTestsCSharp : BaseMultiplyTextTests
{
    protected override string FunctionBody => @"
int i;
    string c = """";
    for(i=0;i<b;i++){ c = c + a; }
    return c;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}