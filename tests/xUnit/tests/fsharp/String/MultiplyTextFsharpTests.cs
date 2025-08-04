using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMultiplyTextFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMultiplyTextFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MultiplyTextFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text"), new FunctionArgument("b", "int") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-text", "multiplyTextFSharp", "'dog ', 3", "= 'dog dog dog '" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyTextFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class MultiplyTextFsharpTestsFSharp : BaseMultiplyTextFsharpTests
{
    protected override string FunctionBody => @"
    let i = 0
    let mutable c: string = """"
    for i in 1 .. b do
        c <- c + a
    c
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}