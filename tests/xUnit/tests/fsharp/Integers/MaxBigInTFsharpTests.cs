using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxBigInTFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxBigInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxBigInTFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "int8", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int8", "maxBigIntFSharp", "", "= int8 '9223372036854775807'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxBigInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class MaxBigInTFsharpTestsFSharp : BaseMaxBigInTFsharpTests
{
    protected override string FunctionBody => @"
Nullable 9223372036854775807L
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}