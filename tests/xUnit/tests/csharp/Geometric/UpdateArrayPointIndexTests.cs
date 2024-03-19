
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayPointIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayPointIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayPointIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "point[]"), new FunctionArgument("desired", "point"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-point-null-1array", "updateArrayPointIndex1", "ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)], POINT(31.43, 32.44), ARRAY[2]", "= CAST(ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), POINT(31.43, 32.44), POINT(40.5,21.3)] AS TEXT)" },
            new object[] { "c#-point-null-2array-arraynull", "updateArrayPointIndex2", "ARRAY[[null::point, null::point], [null::point, POINT(40.5,21.3)]], POINT(31.43, 32.44), ARRAY[1,0]", "= CAST(ARRAY[[null::point, null::point], [POINT(31.43, 32.44), POINT(40.5,21.3)]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPointIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
