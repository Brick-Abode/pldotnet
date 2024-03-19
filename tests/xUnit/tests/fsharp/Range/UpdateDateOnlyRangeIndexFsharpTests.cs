
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class UpdateDateOnlyRangeIndexFsharpTests : PlDotNetTest
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

    public UpdateDateOnlyRangeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateDateOnlyRangeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "DATERANGE[]"), new FunctionArgument("b", "DATERANGE") },
            ReturnType = "DATERANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-daterange-null-1array", "updateDateonlyRangeIndexFSharp1", "ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE], '[2021-05-25,)'::DATERANGE", "= ARRAY['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]" },
        new object[] { "f#-daterange-null-2array", "updateDateonlyRangeIndexFSharp2", "ARRAY[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE", "= ARRAY[['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]" },
        new object[] { "f#-daterange-null-3array", "updateDateonlyRangeIndexFSharp3", "ARRAY[[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]], '[2021-05-25,)'::DATERANGE", "= ARRAY[[['[2021-05-25,)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]]" },
        new object[] { "f#-daterange-null-2array-arraynull", "updateDateonlyRangeIndexFSharp4", "ARRAY[[null::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE", "= ARRAY[['[2021-05-25,)'::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]" },
        new object[] { "f#-daterange-null-3array-arraynull", "updateDateonlyRangeIndexFSharp5", "ARRAY[[[null::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]], '[2021-05-25,)'::DATERANGE", "= ARRAY[[['[2021-05-25,)'::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateDateOnlyRangeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
