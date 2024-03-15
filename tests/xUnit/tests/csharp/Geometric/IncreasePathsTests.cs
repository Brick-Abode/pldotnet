
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreasePathsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlPath orig_value = (NpgsqlPath)flatten_values.GetValue(i);

    NpgsqlPath new_value = new NpgsqlPath(orig_value.Count);
    foreach (NpgsqlPoint polygon_point in orig_value) {
        new_value.Add(new NpgsqlPoint(polygon_point.X + 1, polygon_point.Y + 1));
    }

    flatten_values.SetValue((NpgsqlPath)new_value, i);
}
return flatten_values;
    ";

    public IncreasePathsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreasePaths",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "PATH[]") },
            ReturnType = "PATH[]",
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
            new object[] { "c#-path-null-1array", "IncreasePaths1", "ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]", "= CAST(ARRAY['((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH, null::PATH, '((2.5,3.75),(4.0,5.75),(6.0,6.0))'::PATH] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreasePaths(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
