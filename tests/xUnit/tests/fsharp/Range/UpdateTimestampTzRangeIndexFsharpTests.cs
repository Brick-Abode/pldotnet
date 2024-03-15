
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class UpdateTimestampTzRangeIndexFsharpTests : PlDotNetTest
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

    public UpdateTimestampTzRangeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateTimestampTzRangeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TSTZRANGE[]"), new FunctionArgument("b", "TSTZRANGE") },
            ReturnType = "TSTZRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-tstzrange-null-1array", "updateTimestampTzRangeIndexFSharp1", "ARRAY['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE], '[2021-05-25 14:30 +03,)'::TSTZRANGE", "= ARRAY['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE]" },
        new object[] { "f#-tstzrange-null-2array", "updateTimestampTzRangeIndexFSharp2", "ARRAY[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE", "= ARRAY[['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]" },
        new object[] { "f#-tstzrange-null-3array", "updateTimestampTzRangeIndexFSharp3", "ARRAY[[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]], '[2021-05-25 14:30 +03,)'::TSTZRANGE", "= ARRAY[[['[2021-05-25 14:30 +03,)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]]" },
        new object[] { "f#-tstzrange-null-2array-arraynull", "updateTimestampTzRangeIndexFSharp4", "ARRAY[[null::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE", "= ARRAY[['[2021-05-25 14:30 +03,)'::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]" },
        new object[] { "f#-tstzrange-null-3array-arraynull", "updateTimestampTzRangeIndexFSharp5", "ARRAY[[[null::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]], '[2021-05-25 14:30 +03,)'::TSTZRANGE", "= ARRAY[[['[2021-05-25 14:30 +03,)'::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateTimestampTzRangeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
