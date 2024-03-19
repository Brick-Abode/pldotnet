
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayBoxIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayBoxIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayBoxIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BOX[]"), new FunctionArgument("b", "BOX") },
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
            new object[] {
                "f#-box-null-1array",
                "updateArrayBoxIndexFSharp1",
                "ARRAY[BOX(POINT(0.0,1.0),POINT(5.0,3.0)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))], BOX(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), BOX(POINT(-5.0,4.5),POINT(6.7,12.3)), null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))] AS TEXT)"
            },
            new object[] {
                "f#-box-null-2array-arraynull",
                "updateArrayBoxIndexFSharp2",
                "ARRAY[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]], BOX(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]] AS TEXT)"
            },
            new object[] {
                "f#-box-null-3array-arraynull",
                "updateArrayBoxIndexFSharp3",
                "ARRAY[[[null::BOX, null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]]], BOX(POINT(0.0,1.0),POINT(4.7,9.2))",
                "= CAST(ARRAY[[[BOX(POINT(0.0,1.0),POINT(4.7,9.2)), null::BOX], [null::BOX, BOX(POINT(0.0,1.0),POINT(4.7,9.2))]]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayBoxIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
