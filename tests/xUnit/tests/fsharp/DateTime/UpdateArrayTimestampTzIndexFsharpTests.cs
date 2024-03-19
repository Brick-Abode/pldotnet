
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimestampTzIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayTimestampTzIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimestampTzIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a TIMESTAMP WITH TIME", "ZONE[]"), new FunctionArgument("b TIMESTAMP WITH TIME", "ZONE") },
            ReturnType = "TIMESTAMP WITH TIME ZONE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamptz-1array", "updateArrayTimestamptzIndexFSharp1", "ARRAY[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']" },
        new object[] { "f#-timestamptz-2array", "updateArrayTimestamptzIndexFSharp2", "ARRAY[[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]" },
        new object[] { "f#-timestamptz-3array", "updateArrayTimestamptzIndexFSharp3", "ARRAY[[[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03'], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03'], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]]" },
        new object[] { "f#-timestamptz-null-1array-arraynull", "updateArrayTimestamptzIndexFSharp4", "ARRAY[null::timestamptz, null::timestamptz, null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz, null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']" },
        new object[] { "f#-timestamptz-null-2array-arraynull", "updateArrayTimestamptzIndexFSharp5", "ARRAY[[null::timestamptz, null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]" },
        new object[] { "f#-timestamptz-null-3array-arraynull", "updateArrayTimestamptzIndexFSharp6", "ARRAY[[[null::timestamptz, null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03'", "= ARRAY[[[TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimestampTzIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
