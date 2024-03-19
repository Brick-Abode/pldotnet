
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class AddPointToPolygonFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let mutable polygon = if orig_polygon.HasValue then orig_polygon.Value else NpgsqlPolygon([| NpgsqlPoint(0.0, 0.0); NpgsqlPoint(100.0, 100.0); NpgsqlPoint(200.0, 200.0) |])
    let mutable point = if new_point.HasValue then new_point.Value else NpgsqlPoint(0.0, 0.0)
    let npts = polygon.Count
    let mutable new_polygon = NpgsqlPolygon(npts+1)
    for i in 0 .. npts-1 do
        new_polygon.Add(polygon.[i])
    new_polygon.Add(point)
    new_polygon
    ";

    public AddPointToPolygonFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddPointToPolygonFsharp",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("orig_polygon", "POLYGON"),
                new FunctionArgument("new_point", "POINT")
            },
            ReturnType = "POLYGON",
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
                "f#-polygon",
                "addPointToPolygonFSharp1",
                "POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0))', POINT '(6.5,8.8)'",
                "~= POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0),(6.5,8.8))'"
            },
            new object[] {
                "f#-polygon-null",
                "addPointToPolygonFSharp2",
                "NULL::POLYGON, NULL::POINT",
                "~= POLYGON '((0, 0),(100,100),(200,200),(0,0))'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddPointToPolygonFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
