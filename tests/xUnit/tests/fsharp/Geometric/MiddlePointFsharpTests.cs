
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class MiddlePointFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let pointa = if pointa.HasValue then pointa.Value else  NpgsqlPoint(0.0, 0.0)
let pointb = if pointb.HasValue then pointb.Value else  NpgsqlPoint(0.0, 0.0)
let x = (pointa.X + pointb.X) * 0.5
let y = (pointa.Y + pointb.Y) * 0.5
let new_point = NpgsqlPoint(x, y)
new_point
    ";

    public MiddlePointFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MiddlePointFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") },
            ReturnType = "point",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-point",
                "middlePointFSharp1",
                "POINT(10.0,20.0),POINT(20.0,40.0)",
                "~= POINT(15.0,30.0)"
            },
            new object[] {
                "f#-point-null",
                "middlePointFSharp2",
                "NULL::POINT,POINT(20.0,40.0)",
                "~= POINT(10.0,20.0)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMiddlePointFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
