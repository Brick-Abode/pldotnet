using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateTextFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateTextFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateTextFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "text") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-text", "concatenateTextFSharp1", "'red', 'blue'", "= 'red blue'" }, new object[] { "f#-text-null", "concatenateTextFSharp2", "NULL::TEXT, 'blue'", "= ' blue'" }, new object[] { "f#-text-null", "concatenateTextFSharp3", "NULL::TEXT, NULL::TEXT", "= ' '" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateTextFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ConcatenateTextFsharpTestsFSharp : BaseConcatenateTextFsharpTests
{
    protected override string FunctionBody => @"
a + "" "" + b
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}