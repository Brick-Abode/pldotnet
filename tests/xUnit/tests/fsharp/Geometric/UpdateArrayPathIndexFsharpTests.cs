
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayPathIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayPathIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayPathIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "PATH[]"), new FunctionArgument("b", "PATH") },
            ReturnType = "PATH[]",
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
                "f#-path-null-1array",
                "updateArrayPathIndexFSharp1",
                "ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH",
                "= CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH] AS TEXT)"
            },
            new object[] {
                "f#-path-null-2array-arraynull",
                "updateArrayPathIndexFSharp2",
                "ARRAY[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH",
                "= CAST(ARRAY[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], [null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]] AS TEXT)"
            },
            new object[] {
                "f#-path-null-3array-arraynull",
                "updateArrayPathIndexFSharp3",
                "ARRAY[[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH",
                "= CAST(ARRAY[[['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::PATH], [null::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPathIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
