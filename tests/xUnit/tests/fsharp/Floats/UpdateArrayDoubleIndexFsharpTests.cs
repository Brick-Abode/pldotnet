
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class UpdateArrayDoubleIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayDoubleIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayDoubleIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "float8[]"), new FunctionArgument("b", "float8") },
            ReturnType = "float8[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float8-null-1array", "updateArrayDoubleIndexFSharp1", "ARRAY[4.55535544213::float8, 10.1133254154::float8, null::float8], 9.8321432132", "= ARRAY[9.8321432132::float8, 10.1133254154::float8, null::float8]" },
        new object[] { "f#-float8-null-2array", "updateArrayDoubleIndexFSharp2", "ARRAY[[4.55535544213::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]], 9.8321432132", "= ARRAY[[9.8321432132::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]]" },
        new object[] { "f#-float8-null-2array", "updateArrayDoubleIndexFSharp2", "ARRAY[[[4.55535544213::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]]], 9.8321432132", "= ARRAY[[[9.8321432132::float8, 10.1133254154::float8], [null::float8, 16.16155::float8]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayDoubleIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
