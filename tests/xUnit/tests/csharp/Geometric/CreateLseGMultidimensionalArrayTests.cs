
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateLseGMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlLSeg objects_value = new NpgsqlLSeg(new NpgsqlPoint(25.4, -54.2), new NpgsqlPoint(78.3, 122.31));
NpgsqlLSeg?[, ,] three_dimensional_array = new NpgsqlLSeg?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateLseGMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLseGMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "LSEG[]",
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
            new object[] { "c#-lseg-null-3array-arraynull", "CreateLSEGMultidimensionalArray1", "", "= CAST(ARRAY[[[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))], [null::LSEG, null::LSEG]], [[LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), null::LSEG], [LSEG(POINT(25.4,-54.2),POINT(78.3,122.31)), LSEG(POINT(25.4,-54.2),POINT(78.3,122.31))]]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLseGMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
