using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseBooleanXorFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseBooleanXorFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "BooleanXorFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bool", "BooleanXorFSharp1", "false, false", "is false" }, new object[] { "f#-bool-null", "BooleanXorFSharp2", "NULL::BOOLEAN, NULL::BOOLEAN", "is false" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanXorFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class BooleanXorFsharpTestsFSharp : BaseBooleanXorFsharpTests
{
    protected override string FunctionBody => @"
let a = if a.HasValue then a else false
let b = if b.HasValue then b else false
(a.Value && not b.Value) || (not a.Value && b.Value)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}