
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampTzRangeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else NpgsqlRange<DateTime>(DateTime(2022, 1, 1, 12, 30, 30, DateTimeKind.Utc), true, false, DateTime(2022, 12, 25, 17, 30, 30, DateTimeKind.Utc), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateTime>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
    ";

    public IncreaseTimestampTzRangeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseTimestampTzRangeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TSTZRANGE"), new FunctionArgument("b", "INTEGER") },
            ReturnType = "TSTZRANGE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-tstzrange",
                "IncreaseTimestampTzRangeFSharp1",
                "'[2021-01-01 14:30 -03, 2021-01-04 15:30 +05)'::TSTZRANGE, 1",
                "= '[2021-01-02 14:30 -03, 2021-01-05 15:30 +05)'::TSTZRANGE"
            },
            new object[] {
                "f#-tstzrange",
                "IncreaseTimestampTzRangeFSharp2",
                "'[, 2021-01-01 15:30 -03)'::TSTZRANGE, 3",
                "= '[, 2021-01-04 15:30 -03)'::TSTZRANGE"
            },
            new object[] {
                "f#-tstzrange",
                "IncreaseTimestampTzRangeFSharp3",
                "'[,)'::TSTZRANGE, 3",
                "= '(,)'::TSTZRANGE"
            },
            new object[] {
                "f#-tstzrange",
                "IncreaseTimestampTzRangeFSharp4",
                "'(2021-01-01 14:30 -03, 2021-01-04 15:30 +05]'::TSTZRANGE, 3",
                "= '(2021-01-04 14:30 -03, 2021-01-07 15:30 +05]'::TSTZRANGE"
            },
            new object[] {
                "f#-tstzrange-null",
                "IncreaseTimestampTzRangeFSharp5",
                "NULL::TSTZRANGE, 3",
                @"= '[""2022-01-04 12:30:30+00"",""2022-12-28 17:30:30+00"")'::TSTZRANGE"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampTzRangeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
