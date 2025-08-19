using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnDoubleFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnDoubleFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnDoubleFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "float8", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-float8", "returnDoubleFSharp", "", "= float8 '11.0050000000005'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnDoubleFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class ReturnDoubleFsharpTestsFSharp : BaseReturnDoubleFsharpTests
{
    protected override string FunctionBody => @"
11.0050000000005
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}