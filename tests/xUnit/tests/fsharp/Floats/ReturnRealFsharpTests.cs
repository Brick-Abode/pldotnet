using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnRealFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnRealFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnRealFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "real", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-float4", "returnRealFSharp", "", "= real '1.50055'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnRealFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class ReturnRealFsharpTestsFSharp : BaseReturnRealFsharpTests
{
    protected override string FunctionBody => @"
1.50055f
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}