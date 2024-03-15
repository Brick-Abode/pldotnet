
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayCircleIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayCircleIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayCircleIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "CIRCLE[]"), new FunctionArgument("desired", "CIRCLE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-circle-null-1array", "updateArrayCircleIndex1", "ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)], CIRCLE(POINT(0.0,1.0), 2), ARRAY[2]", "= CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(0.0,1.0),4.5)] AS TEXT)" },
            new object[] { "c#-circle-null-2array-arraynull", "updateArrayCircleIndex2", "ARRAY[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]], CIRCLE(POINT(0.0,1.0), 2), ARRAY[1,0]", "= CAST(ARRAY[[null::CIRCLE, null::CIRCLE], [CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(0.0,1.0),4.5)]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayCircleIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
