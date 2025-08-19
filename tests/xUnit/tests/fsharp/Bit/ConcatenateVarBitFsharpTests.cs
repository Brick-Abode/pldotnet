using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateVarBitFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateVarBitFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateVarBitFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a BIT", "VARYING"), new FunctionArgument("b BIT", "VARYING") }, ReturnType = "BIT VARYING", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-varbit", "concatenatevarbitfsharp1", "'1001110001000'::BIT VARYING, '111010111101111000'::BIT VARYING", "= '1001110001000111010111101111000'::BIT VARYING" }, new object[] { "f#-varbit", "concatenatevarbitfsharp2", "'1001110001000'::BIT(10), '111010111101111000'::BIT VARYING", "= '1001110001111010111101111000'::BIT VARYING" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarBitFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class ConcatenateVarBitFsharpTestsFSharp : BaseConcatenateVarBitFsharpTests
{
    protected override string FunctionBody => @"
    let result = new BitArray(a.Length + b.Length)
    for i in 0 .. a.Length - 1 do
        result.[i] <- a.[i]
    for i in a.Length .. result.Length - 1 do
        result.[i] <- b.[i - a.Length]
    result
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}