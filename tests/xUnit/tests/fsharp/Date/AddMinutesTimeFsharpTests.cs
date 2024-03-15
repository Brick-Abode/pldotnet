
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Date")]
public class AddMinutesTimeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (orig_time.HasValue, minutes.HasValue) with
        | (true, true) ->
            Nullable((orig_time.Value).AddMinutes(double minutes.Value))
        | (true, false) -> Nullable(orig_time.Value)
        | (false, true) -> Nullable((TimeOnly(0, 0, 0)).AddMinutes(double minutes.Value))
        | (false, false) -> Nullable(TimeOnly(0, 0, 0))
    ";

    public AddMinutesTimeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddMinutesTimeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_time", "TIME"), new FunctionArgument("minutes", "int") },
            ReturnType = "TIME",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-date", "addMinutesTimeFSharp1", "TIME '05:30 PM', CAST(75 AS int)", "= TIME '06:45 PM'" },
        new object[] { "f#-date-null", "addMinutesTimeFSharp2", "TIME '05:30 PM', NULL::int", "= TIME '05:30 PM'" },
        new object[] { "f#-date-null", "addMinutesTimeFSharp3", "NULL::TIME, CAST(75 AS int)", "= TIME '01:15:00'" },
        new object[] { "f#-date-null", "addMinutesTimeFSharp4", "NULL::TIME, NULL::int", "= TIME '00:00:00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddMinutesTimeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
