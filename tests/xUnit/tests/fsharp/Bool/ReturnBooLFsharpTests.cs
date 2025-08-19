using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnBooLFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnBooLFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnBooLFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bool", "returnBoolFSharp", "", "is false" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBooLFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class ReturnBooLFsharpTestsFSharp : BaseReturnBooLFsharpTests
{
    protected override string FunctionBody => @"
false
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}