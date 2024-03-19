
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreatePointMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlPoint>, 3, 3, 1)
let objects_value = NpgsqlPoint(2.4, 8.2)
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
    ";

    public CreatePointMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreatePointMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "point[]",
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
            new object[] { "f#-point-null-3array-arraynull", "CreatePointMultidimensionalArrayFSharp1", "", "= CAST(ARRAY[[[POINT(2.4,8.2)], [POINT(0,0)], [POINT(0,0)]], [[POINT(0,0)], [POINT(2.4,8.2)], [POINT(0,0)]], [[POINT(0,0)], [POINT(0,0)], [POINT(2.4,8.2)]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreatePointMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
