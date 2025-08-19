using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateCharsFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateCharsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateCharsFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR"), new FunctionArgument("b", "BPCHAR"), new FunctionArgument("c", "BPCHAR") }, ReturnType = "BPCHAR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bpchar", "concatenateCharsFSharp1", "'hello'::BPCHAR, 'beautiful'::BPCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::BPCHAR" }, new object[] { "f#-bpchar-null", "concatenateCharsFSharp2", "NULL::BPCHAR, 'beautiful'::BPCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::BPCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateCharsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ConcatenateCharsFsharpTestsFSharp : BaseConcatenateCharsFsharpTests
{
    protected override string FunctionBody => @"
(a + "" "" + b + "" "" + c).ToUpper()
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}