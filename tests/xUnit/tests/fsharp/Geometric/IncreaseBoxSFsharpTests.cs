
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreaseBoxSFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let flatten_values = Array.CreateInstance(typeof<NpgsqlBox>, values_array.Length)
ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
for i in 0 .. flatten_values.Length - 1 do
    if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
        ()
    else
        let orig_value = flatten_values.GetValue(i) :?> NpgsqlBox
        let new_value = NpgsqlBox(NpgsqlPoint(orig_value.UpperRight.X + 1., orig_value.UpperRight.Y + 1.), NpgsqlPoint(orig_value.LowerLeft.X + 1., orig_value.LowerLeft.Y + 1.))
        flatten_values.SetValue(new_value, i)
flatten_values
    ";

    public IncreaseBoxSFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseBoxSFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BOX[]") },
            ReturnType = "BOX[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-box-null-1array", "IncreaseBoxsFSharp1", "ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]", "= CAST(ARRAY[BOX(POINT(1.0,2.0),POINT(6.0,4.0)), BOX(POINT(-4.0,5.5),POINT(7.7,13.3)), BOX(POINT(1,1),POINT(1,1)), BOX(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseBoxSFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
