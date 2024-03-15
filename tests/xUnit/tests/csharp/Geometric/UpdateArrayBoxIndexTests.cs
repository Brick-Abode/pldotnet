
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayBoxIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayBoxIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayBoxIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BOX[]"), new FunctionArgument("desired", "BOX"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-box-null-1array", "updateArrayBoxIndex1", "ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))], BOX(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[2]", "= CAST(ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT)" },
            new object[] { "c#-box-null-2array-arraynull", "updateArrayBoxIndex2", "ARRAY[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]], BOX(POINT(0.0,1.0),POINT(4.7,9.2)), ARRAY[1,0]", "= CAST(ARRAY[[null::BOX, null::BOX], [BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayBoxIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
