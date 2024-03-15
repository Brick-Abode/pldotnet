
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseBoxSTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlBox orig_value = (NpgsqlBox)flatten_values.GetValue(i);
    NpgsqlBox new_value = new NpgsqlBox(new NpgsqlPoint(orig_value.UpperRight.X + 1, orig_value.UpperRight.Y + 1), new NpgsqlPoint(orig_value.LowerLeft.X + 1, orig_value.LowerLeft.Y + 1));

    flatten_values.SetValue((NpgsqlBox)new_value, i);
}
return flatten_values;
    ";

    public IncreaseBoxSTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseBoxS",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BOX[]") },
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
            new object[] {
                "c#-box-null-1array",
                "IncreaseBoxs1",
                "ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]",
                "= CAST(ARRAY[BOX(POINT(1.0,2.0),POINT(6.0,4.0)), BOX(POINT(-4.0,5.5),POINT(7.7,13.3)), null::BOX, BOX(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseBoxS(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
