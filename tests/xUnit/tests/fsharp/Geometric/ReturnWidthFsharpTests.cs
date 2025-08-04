using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnWidthFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnWidthFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnWidthFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") }, ReturnType = "float8", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-box", "returnWidthFSharp", "POINT '(0.025988, 1.021653)', POINT '(2.052787, 3.005716)'", "= float8 '2.026799'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnWidthFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class ReturnWidthFsharpTestsFSharp : BaseReturnWidthFsharpTests
{
    protected override string FunctionBody => @"
    let new_box = NpgsqlBox(high, low)
    Math.Abs(new_box.Width)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}