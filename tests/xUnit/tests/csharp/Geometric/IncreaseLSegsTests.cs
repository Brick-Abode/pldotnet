
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseLSegsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlLSeg orig_value = (NpgsqlLSeg)flatten_values.GetValue(i);
    NpgsqlLSeg new_value = new NpgsqlLSeg(new NpgsqlPoint(orig_value.Start.X + 1, orig_value.Start.Y + 1), new NpgsqlPoint(orig_value.End.X + 1, orig_value.End.Y + 1));

    flatten_values.SetValue((NpgsqlLSeg)new_value, i);
}
return flatten_values;
    ";

    public IncreaseLSegsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseLSegs",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LSEG[]") },
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
            new object[] { "c#-lseg-null-1array", "IncreaseLSEGs1", "ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]", "= CAST(ARRAY[LSEG(POINT(1.0,2.0),POINT(6.0,4.0)), LSEG(POINT(-4.0,5.5),POINT(7.7,13.3)), null::LSEG, LSEG(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseLSegs(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
