using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseGetMinimumDistanceFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseGetMinimumDistanceFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "GetMinimumDistanceFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_line", "LINE"), new FunctionArgument("orig_point", "POINT") }, ReturnType = "float8", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-line", "getMinimumDistanceFSharp", "LINE '{4.0, 6.0, 2.0}', POINT(3.0,-6.0)", "= float8 '3.05085107923876'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetMinimumDistanceFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class GetMinimumDistanceFsharpTestsFSharp : BaseGetMinimumDistanceFsharpTests
{
    protected override string FunctionBody => @"
let a = orig_line.A
let b = orig_line.B
let c = orig_line.C
Math.Abs((a*orig_point.X + b*orig_point.Y + c)/Math.Sqrt(a*a+b*b))
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}