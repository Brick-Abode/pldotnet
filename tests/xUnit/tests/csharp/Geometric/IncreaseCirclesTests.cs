
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseCirclesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlCircle orig_value = (NpgsqlCircle)flatten_values.GetValue(i);
    NpgsqlCircle new_value = new NpgsqlCircle(orig_value.Center, orig_value.Radius + 1);

    flatten_values.SetValue((NpgsqlCircle)new_value, i);
}
return flatten_values;
    ";

    public IncreaseCirclesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseCircles",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "CIRCLE[]") },
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
            new object[] { "c#-circle-null-1array", "IncreaseCircles1", "ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]", "= CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 3.5), CIRCLE(POINT(-5.0,4.5), 5), null::CIRCLE, CIRCLE(POINT(0.0,1.0),5.5)] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseCircles(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
