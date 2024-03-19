
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class SumRealArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_floats = Array.CreateInstance(typeof(object), floats.Length);
ArrayManipulation.FlatArray(floats, ref flatten_floats);
float float_sum = 0;
for(int i = 0; i < flatten_floats.Length; i++)
{
    if (flatten_floats.GetValue(i) == null)
        continue;
    float_sum = float_sum + (float)flatten_floats.GetValue(i);
}
return float_sum;
    ";

    public SumRealArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumRealArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("floats", "real[]") },
            ReturnType = "real",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float4-null-1array", "sumRealArray1", "ARRAY[1.50055::real, 2.30300::real, 4.52123::real, 7.41234::real, null::real]", "= '15.737121'" },
        new object[] { "c#-float4-null-2array-arraynull", "sumRealArray2", "ARRAY[[1.50055::real, 2.30300::real], [4.52123::real, 7.41234::real], [null::real, null::real]]", "= '15.737121'" },
        new object[] { "c#-float4-null-3array-arraynull", "sumRealArray3", "ARRAY[[[1.50055::real, 2.30300::real], [4.52123::real, 7.41234::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [null::real, null::real], [9.52123::real, 11.41234::real]]]", "= '52.474243'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumRealArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
