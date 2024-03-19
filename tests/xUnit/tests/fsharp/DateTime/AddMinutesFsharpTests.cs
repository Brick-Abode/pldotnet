
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class AddMinutesFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (orig_time.HasValue, min_to_add.HasValue) with
        | (true, true) ->
            Nullable((orig_time.Value).AddMinutes(double min_to_add.Value))
        | (true, false) -> Nullable(orig_time.Value)
        | (false, true) -> Nullable((TimeOnly(0, 0, 0)).AddMinutes(double min_to_add.Value))
        | (false, false) -> Nullable(TimeOnly(0, 0, 0))
    ";

    public AddMinutesFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddMinutesFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_time", "TIME"), new FunctionArgument("min_to_add", "INT") },
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
            new object[] { "f#-time", "addMinutesFSharp1", "TIME '05:30 PM', 75", "= TIME '06:45 PM'" },
        new object[] { "f#-time-null", "addMinutesFSharp2", "NULL::TIME, 75", "= TIME '01:15:00'" },
        new object[] { "f#-time-null", "addMinutesFSharp1", "TIME '05:30 PM', NULL", "= TIME '17:30'" },
        new object[] { "f#-time-null", "addMinutesFSharp2", "NULL::TIME, NULL", "= TIME '00:00:00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddMinutesFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
