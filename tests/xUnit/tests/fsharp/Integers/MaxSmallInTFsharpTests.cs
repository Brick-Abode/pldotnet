using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxSmallInTFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxSmallInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxSmallInTFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "int2", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int2", "maxSmallIntFSharp", "", "= int2 '32767'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxSmallInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxSmallInTFsharpTestsFSharp : BaseMaxSmallInTFsharpTests
{
    protected override string FunctionBody => @"
Nullable (32767s)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}