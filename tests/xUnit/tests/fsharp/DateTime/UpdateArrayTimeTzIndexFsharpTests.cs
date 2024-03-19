
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimeTzIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayTimeTzIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimeTzIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TIMETZ[]"), new FunctionArgument("b", "TIMETZ") },
            ReturnType = "TIMETZ[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timetz-1array", "updateArrayTimetzIndexFSharp1", "ARRAY[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00'], TIMETZ '02:30-05:00'", "= ARRAY[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00']" },
        new object[] { "f#-timetz-2array", "updateArrayTimetzIndexFSharp2", "ARRAY[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00'", "= ARRAY[[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']]" },
        new object[] { "f#-timetz-3array", "updateArrayTimetzIndexFSharp3", "ARRAY[[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']]], TIMETZ '02:30-05:00'", "= ARRAY[[[TIMETZ '02:30-05:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']]]" },
        new object[] { "f#-timetz-null-1array-arraynull", "updateArrayTimetzIndexFSharp4", "ARRAY[null::TIMETZ, null::TIMETZ, null::timetz, TIMETZ '22:30-03:00'], TIMETZ '02:30-05:00'", "= ARRAY[TIMETZ '02:30-05:00', null::TIMETZ, null::timetz, TIMETZ '22:30-03:00']" },
        new object[] { "f#-timetz-null-2array-arraynull", "updateArrayTimetzIndexFSharp5", "ARRAY[[null::TIMETZ, null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00'", "= ARRAY[[TIMETZ '02:30-05:00', null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']]" },
        new object[] { "f#-timetz-null-3array-arraynull", "updateArrayTimetzIndexFSharp6", "ARRAY[[[null::TIMETZ, null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']]], TIMETZ '02:30-05:00'", "= ARRAY[[[TIMETZ '02:30-05:00', null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimeTzIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
