
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimestampIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayTimestampIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimestampIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TIMESTAMP[]"), new FunctionArgument("b", "TIMESTAMP") },
            ReturnType = "TIMESTAMP[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamp-1array", "updateArrayTimestampIndexFSharp1", "ARRAY[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']" },
        new object[] { "f#-timestamp-2array", "updateArrayTimestampIndexFSharp2", "ARRAY[[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]" },
        new object[] { "f#-timestamp-3array", "updateArrayTimestampIndexFSharp3", "ARRAY[[[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[[[TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM'], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]]" },
        new object[] { "f#-timestamp-null-1array-arraynull", "updateArrayTimestampIndexFSharp4", "ARRAY[null::timestamp, null::timestamp, null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp, null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']" },
        new object[] { "f#-timestamp-null-2array-arraynull", "updateArrayTimestampIndexFSharp5", "ARRAY[[null::timestamp, null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]" },
        new object[] { "f#-timestamp-null-3array-arraynull", "updateArrayTimestampIndexFSharp6", "ARRAY[[[null::timestamp, null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]], TIMESTAMP '2025-10-19 10:23:54 PM'", "= ARRAY[[[TIMESTAMP '2025-10-19 10:23:54 PM', null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimestampIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
