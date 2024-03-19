
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimeIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayTimeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TIME[]"), new FunctionArgument("b", "TIME") },
            ReturnType = "TIME[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-time-1array", "updateArrayTimeIndexFSharp1", "ARRAY[TIME '05:30 PM', TIME '06:30 PM', null::time, TIME '09:30 AM'], TIME '5:45 AM'", "= ARRAY[TIME '5:45 AM', TIME '06:30 PM', null::time, TIME '09:30 AM']" },
        new object[] { "f#-time-2array", "updateArrayTimeIndexFSharp2", "ARRAY[[TIME '05:30 PM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']], TIME '5:45 AM'", "= ARRAY[[TIME '5:45 AM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']]" },
        new object[] { "f#-time-3array", "updateArrayTimeIndexFSharp3", "ARRAY[[[TIME '05:30 PM'], [TIME '06:30 PM']], [[null::time], [TIME '09:30 AM']]], TIME '5:45 AM'", "= ARRAY[[[TIME '5:45 AM'], [TIME '06:30 PM']], [[null::time], [TIME '09:30 AM']]]" },
        new object[] { "f#-time-null-1array-arraynull", "updateArrayTimeIndexFSharp4", "ARRAY[null::TIME, null::TIME, null::time, TIME '09:30 AM'], TIME '5:45 AM'", "= ARRAY[TIME '5:45 AM', null::TIME, null::time, TIME '09:30 AM']" },
        new object[] { "f#-time-null-2array-arraynull", "updateArrayTimeIndexFSharp5", "ARRAY[[null::TIME, null::TIME], [null::time, TIME '09:30 AM']], TIME '5:45 AM'", "= ARRAY[[TIME '5:45 AM', null::TIME], [null::time, TIME '09:30 AM']]" },
        new object[] { "f#-time-null-3array-arraynull", "updateArrayTimeIndexFSharp6", "ARRAY[[[null::TIME], [null::TIME]], [[null::time], [TIME '09:30 AM']]], TIME '5:45 AM'", "= ARRAY[[[TIME '5:45 AM'], [null::TIME]], [[null::time], [TIME '09:30 AM']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
