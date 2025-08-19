using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxIntegerFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxIntegerFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxIntegerFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "int4", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int4", "maxIntegerFSharp", "", "= int4 '2147483647'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxIntegerFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxIntegerFsharpTestsFSharp : BaseMaxIntegerFsharpTests
{
    protected override string FunctionBody => @"
Nullable 2147483647
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}