
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayCircleIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
    ";

    public UpdateArrayCircleIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayCircleIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "CIRCLE[]"), new FunctionArgument("b", "CIRCLE") },
            ReturnType = "CIRCLE[]",
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
            new object[] {
                "f#-circle-null-1array",
                "updateArrayCircleIndexFSharp1",
                "ARRAY[CIRCLE(POINT(0.0,1.0), 2.5), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)], CIRCLE(POINT(0.0,1.0), 2)",
                "= CAST(ARRAY[CIRCLE(POINT(0.0,1.0), 2), CIRCLE(POINT(-5.0,4.5), 4), null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)] AS TEXT)"
            },
            new object[] {
                "f#-circle-null-2array-arraynull",
                "updateArrayCircleIndexFSharp2",
                "ARRAY[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]], CIRCLE(POINT(0.0,1.0), 2)",
                "= CAST(ARRAY[[CIRCLE(POINT(0.0,1.0), 2), null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]] AS TEXT)"
            },
            new object[] {
                "f#-circle-null-3array-arraynull",
                "updateArrayCircleIndexFSharp3",
                "ARRAY[[[null::CIRCLE, null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]]], CIRCLE(POINT(0.0,1.0), 2)",
                "= CAST(ARRAY[[[CIRCLE(POINT(0.0,1.0), 2), null::CIRCLE], [null::CIRCLE, CIRCLE(POINT(0.0,1.0),4.5)]]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayCircleIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
