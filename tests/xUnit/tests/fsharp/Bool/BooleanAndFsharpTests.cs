using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseBooleanAndFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseBooleanAndFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "BooleanAndFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bool", "BooleanAndFSharp1", "true, true", "is true" }, new object[] { "f#-bool-null", "BooleanAndFSharp2", "NULL::BOOLEAN, true", "is false" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanAndFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class BooleanAndFsharpTestsFSharp : BaseBooleanAndFsharpTests
{
    protected override string FunctionBody => @"
let a = if a.HasValue then a else false
let b = if b.HasValue then b else false
a.Value && b.Value
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}