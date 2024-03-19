
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayLseGIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayLseGIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayLseGIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "LSEG[]"), new FunctionArgument("b", "LSEG") },
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
                "updateArrayLSEGIndexFSharp1",
                "ARRAY[LSEG(POINT(0.0,1.0),POINT(5.0,3.0)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), LSEG(POINT(-5.0,4.5),POINT(6.7,12.3)), null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT)"
            },
            new object[] {
                "f#-lseg-null-2array-arraynull",
                "updateArrayLSEGIndexFSharp2",
                "ARRAY[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT)"
            },
            new object[] {
                "f#-lseg-null-3array-arraynull",
                "updateArrayLSEGIndexFSharp3",
                "ARRAY[[[null::LSEG, null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]]], LSEG(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[[[LSEG(POINT(0.0,1.0),POINT(4.7,9.2)), null::LSEG], [null::LSEG, LSEG(POINT(0.0,1.0),POINT(4.7,9.2))]]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayLseGIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
