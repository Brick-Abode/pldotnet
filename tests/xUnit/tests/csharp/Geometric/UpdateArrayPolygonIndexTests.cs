
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayPolygonIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayPolygonIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayPolygonIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "POLYGON[]"), new FunctionArgument("desired", "POLYGON"), new FunctionArgument("index", "integer[]") },
            ReturnType = "POLYGON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT"
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-polygon-null-1array", "updateArrayPolygonIndex1", "ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[2]", "= CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON] AS TEXT)" },
            new object[] { "c#-polygon-null-2array-arraynull", "updateArrayPolygonIndex2", "ARRAY[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[1,0]", "= CAST(ARRAY[[null::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPolygonIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
