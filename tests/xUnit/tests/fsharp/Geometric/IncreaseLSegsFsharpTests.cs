
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreaseLSegsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let flatten_values = Array.CreateInstance(typeof<NpgsqlLSeg>, values_array.Length)
ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
for i in 0 .. flatten_values.Length - 1 do
    if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
        ()
    else
        let orig_value = flatten_values.GetValue(i) :?> NpgsqlLSeg
        let new_value = NpgsqlLSeg(NpgsqlPoint(orig_value.Start.X + 1., orig_value.Start.Y + 1.), NpgsqlPoint(orig_value.End.X + 1., orig_value.End.Y + 1.))
        flatten_values.SetValue(new_value, i)
flatten_values
    ";

    public IncreaseLSegsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseLSegsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LSEG[]") },
            ReturnType = "LSEG[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
            CastFunctionAs = "TEXT"
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-lseg-null-1array",
                "IncreaseLSEGsFSharp1",
                "ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]",
                "= CAST(ARRAY[LSEG(POINT(1.0,2.0),POINT(6.0,4.0)), LSEG(POINT(-4.0,5.5),POINT(7.7,13.3)), LSEG(POINT(1,1),POINT(1,1)), LSEG(POINT(1.0,2.0),POINT(5.7,10.2))] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseLSegsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
