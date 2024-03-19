
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreatePolygonMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlPolygon objects_value = new NpgsqlPolygon(new NpgsqlPoint(1.5, 2.75), new NpgsqlPoint(3.0, 4.75), new NpgsqlPoint(5.0, 5.0));
NpgsqlPolygon?[, ,] three_dimensional_array = new NpgsqlPolygon?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreatePolygonMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreatePolygonMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "POLYGON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-polygon-null-3array-arraynull", "CreatePolygonMultidimensionalArray1", "", "= CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], [null::POLYGON, null::POLYGON]], [['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreatePolygonMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
