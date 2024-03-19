
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateLseGMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlLSeg>, 3, 3, 1)
let objects_value = NpgsqlLSeg(NpgsqlPoint(25.4, -54.2), NpgsqlPoint(78.3, 122.31))
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
    ";

    public CreateLseGMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLseGMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "LSEG[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT"
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-lseg-null-3array-arraynull", "CreateLSEGMultidimensionalArrayFSharp1", "", "= CAST(ARRAY[[[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))],[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(0,0),POINT(0,0))]],[[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))],[LSEG(POINT(0,0),POINT(0,0))]],[[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(0,0),POINT(0,0))],[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLseGMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
