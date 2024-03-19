
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateCircleMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlCircle objects_value = new NpgsqlCircle(new NpgsqlPoint(25.4, -54.2), 3);
NpgsqlCircle?[, ,] three_dimensional_array = new NpgsqlCircle?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateCircleMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateCircleMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "CIRCLE[]",
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
            new object[] { "c#-circle-null-3array-arraynull", "CreateCircleMultidimensionalArray1", "", "= CAST(ARRAY[[[CIRCLE(POINT(25.4,-54.2),3), CIRCLE(POINT(25.4,-54.2),3)], [null::CIRCLE, null::CIRCLE]], [[CIRCLE(POINT(25.4,-54.2),3), null::CIRCLE], [CIRCLE(POINT(25.4,-54.2),3), CIRCLE(POINT(25.4,-54.2),3)]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateCircleMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
