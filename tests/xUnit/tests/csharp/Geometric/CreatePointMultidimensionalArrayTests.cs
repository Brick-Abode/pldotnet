
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreatePointMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlPoint objects_value = new NpgsqlPoint(2.4, 8.2);
NpgsqlPoint?[, ,] three_dimensional_array = new NpgsqlPoint?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreatePointMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreatePointMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "point[]",
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
            new object[] { "c#-point-null-3array-arraynull", "CreatePointMultidimensionalArray1", "", "= CAST(ARRAY[[[POINT(2.4,8.2), POINT(2.4,8.2)], [null::point, null::point]], [[POINT(2.4,8.2), null::point], [POINT(2.4,8.2), POINT(2.4,8.2)]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreatePointMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
