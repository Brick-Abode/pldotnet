using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMultIntegersFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMultIntegersFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MultIntegersFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int4"), new FunctionArgument("b", "int4") }, ReturnType = "int4", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int4", "multIntegersFSharp", "15, 15", "= int4 '225'" }, new object[] { "f#-int4", "multIntegersFSharp", "50, 75", "= int4 '3750'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultIntegersFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MultIntegersFsharpTestsFSharp : BaseMultIntegersFsharpTests
{
    protected override string FunctionBody => @"
Nullable (a*b)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}