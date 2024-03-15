
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class UpdateTimestampRangeIndexFsharpTests : PlDotNetTest
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

    public UpdateTimestampRangeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateTimestampRangeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TSRANGE[]"), new FunctionArgument("b", "TSRANGE") },
            ReturnType = "TSRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-tsrange-null-1array", "updateTimestampRangeIndexFSharp1", "ARRAY['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE], '[2021-05-25 14:30,)'::TSRANGE", "= ARRAY['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE]" },
        new object[] { "f#-tsrange-null-2array", "updateTimestampRangeIndexFSharp2", "ARRAY[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE", "= ARRAY[['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]" },
        new object[] { "f#-tsrange-null-3array", "updateTimestampRangeIndexFSharp3", "ARRAY[[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]], '[2021-05-25 14:30,)'::TSRANGE", "= ARRAY[[['[2021-05-25 14:30,)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]]" },
        new object[] { "f#-tsrange-null-2array-arraynull", "updateTimestampRangeIndexFSharp4", "ARRAY[[null::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE", "= ARRAY[['[2021-05-25 14:30,)'::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]" },
        new object[] { "f#-tsrange-null-3array-arraynull", "updateTimestampRangeIndexFSharp5", "ARRAY[[[null::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]], '[2021-05-25 14:30,)'::TSRANGE", "= ARRAY[[['[2021-05-25 14:30,)'::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateTimestampRangeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
