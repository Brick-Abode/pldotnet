
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayLineIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayLineIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayLineIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("A", "LINE[]"), new FunctionArgument("b", "LINE") },
            ReturnType = "LINE[]",
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
                "f#-line-1array",
                "updateArrayLineIndexFSharp1",
                "ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'], LINE '{-1.5,2.75,-3.25}'",
                "= CAST(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'] AS TEXT)"
            },
            new object[] {
                "f#-line-null-2array-arraynull",
                "updateArrayLineIndexFSharp2",
                "ARRAY[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']], LINE '{-1.5,2.75,-3.25}'",
                "= CAST(ARRAY[[LINE '{-1.5,2.75,-3.25}', null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']] AS TEXT)"
            },
            new object[] {
                "f#-line-null-3array-arraynull",
                "updateArrayLineIndexFSharp3",
                "ARRAY[[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']]], LINE '{-1.5,2.75,-3.25}'",
                "= CAST(ARRAY[[[LINE '{-1.5,2.75,-3.25}', null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayLineIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
