using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseAddGoodbyeFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseAddGoodbyeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "AddGoodbyeFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR") }, ReturnType = "BPCHAR", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bpchar", "testingBpCharFSharp", "'HELLO!'", "= 'HELLO! Goodbye ^.^'::BPCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddGoodbyeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class AddGoodbyeFsharpTestsFSharp : BaseAddGoodbyeFsharpTests
{
    protected override string FunctionBody => @"
a + "" Goodbye ^.^""
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}