
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class AddPointToPolygonTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_polygon == null)
    orig_polygon = new NpgsqlPolygon(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100), new NpgsqlPoint(200, 200));

if (new_point == null)
    new_point = new NpgsqlPoint(0, 0);

int npts = ((NpgsqlPolygon)orig_polygon).Count;
NpgsqlPolygon new_polygon = new NpgsqlPolygon(npts+1);
for(int i = 0; i < npts; i++)
{
    new_polygon.Add(((NpgsqlPolygon)orig_polygon)[i]);
}
new_polygon.Add(((NpgsqlPoint)new_point));
return new_polygon;
    ";

    public AddPointToPolygonTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddPointToPolygon",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("orig_polygon", "POLYGON"),
                new FunctionArgument("new_point", "POINT")
            },
            ReturnType = "POLYGON",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-polygon", "addPointToPolygon1", "POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0))', POINT '(6.5,8.8)'", "~= POLYGON '((1.5,2.75),(3.0,4.75),(5.0,5.0),(6.5,8.8))'" },
            new object[] { "c#-polygon-null", "addPointToPolygon2", "NULL::POLYGON, NULL::POINT", "~= POLYGON '((0, 0),(100,100),(200,200),(0,0))'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddPointToPolygon(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
