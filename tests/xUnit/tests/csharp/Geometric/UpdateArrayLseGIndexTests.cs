
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayLseGIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayLseGIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayLseGIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LSEG[]"), new FunctionArgument("desired", "LSEG"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-lseg-null-1array", "updateArrayLSEGIndex1", "ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))], LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[2]", "= CAST(ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT)" },
            new object[] { "c#-lseg-null-2array-arraynull", "updateArrayLSEGIndex2", "ARRAY[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[1,0]", "= CAST(ARRAY[[null::LSEG, null::LSEG], [LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayLseGIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
