
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateBoxMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlBox objects_value = new NpgsqlBox(new NpgsqlPoint(25.4, -54.2), new NpgsqlPoint(78.3, 122.31));
NpgsqlBox?[, ,] three_dimensional_array = new NpgsqlBox?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateBoxMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBoxMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "BOX[]",
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
            new object[] {
              "c#-box-null-3array-arraynull",
              "CreateBoxMultidimensionalArray1",
              "",
              "= CAST(ARRAY[[[BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), BOX(POINT(25.4,-54.2),POINT(78.3,122.31))], [null::BOX, null::BOX]], [[BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), null::BOX], [BOX(POINT(25.4,-54.2),POINT(78.3,122.31)), BOX(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBoxMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
