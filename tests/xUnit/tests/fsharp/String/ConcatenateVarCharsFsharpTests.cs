using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateVarCharsFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateVarCharsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateVarCharsFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "VARCHAR"), new FunctionArgument("c", "BPCHAR") }, ReturnType = "VARCHAR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-varchar", "concatenateVarCharsFSharp1", "'hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::VARCHAR" }, new object[] { "f#-varchar-null", "concatenateVarCharsFSharp2", "NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::VARCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarCharsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ConcatenateVarCharsFsharpTestsFSharp : BaseConcatenateVarCharsFsharpTests
{
    protected override string FunctionBody => @"
(a + "" "" + b + "" "" + c).ToUpper()
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}