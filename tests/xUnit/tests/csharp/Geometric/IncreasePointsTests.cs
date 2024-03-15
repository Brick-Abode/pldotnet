
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreasePointsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlPoint orig_value = (NpgsqlPoint)flatten_values.GetValue(i);
    NpgsqlPoint new_value = new NpgsqlPoint(orig_value.X + 1, orig_value.Y + 1);

    flatten_values.SetValue((NpgsqlPoint)new_value, i);
}
return flatten_values;
    ";

    public IncreasePointsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreasePoints",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "point[]") },
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
            new object[] { "c#-point-null-1array", "IncreasePoints1", "ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)]", "= CAST(ARRAY[POINT(11.0,21.0), POINT(31.0,56.0), null::point, POINT(41.5,22.3)] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreasePoints(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
