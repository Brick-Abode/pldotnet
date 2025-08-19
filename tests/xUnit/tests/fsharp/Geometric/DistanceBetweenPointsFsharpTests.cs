using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseDistanceBetweenPointsFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseDistanceBetweenPointsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "DistanceBetweenPointsFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") }, ReturnType = "float8", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-point", "distanceBetweenPointsFSharp", "POINT(1.5,2.75), POINT(3.0,4.75)", "= float8 '2.5'" }, new object[] { "f#-point-null", "distanceBetweenPointsFSharp", "POINT(3.0,4.0), NULL::POINT", "= float8 '5'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDistanceBetweenPointsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class DistanceBetweenPointsFsharpTestsFSharp : BaseDistanceBetweenPointsFsharpTests
{
    protected override string FunctionBody => @"
let pointa = if pointa.HasValue then pointa.Value else  NpgsqlPoint(0.0, 0.0)
let pointb = if pointb.HasValue then pointb.Value else  NpgsqlPoint(0.0, 0.0)
let dif_x = pointa.X - pointb.X
let dif_y = pointa.Y - pointb.Y
let distance = Math.Sqrt(dif_x * dif_x + dif_y * dif_y)
distance
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}