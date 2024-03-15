
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampRangeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else NpgsqlRange<DateTime>(DateTime(2022, 1, 1, 12, 30, 30), true, false, DateTime(2022, 12, 25, 17, 30, 30), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateTime>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
    ";

    public IncreaseTimestampRangeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseTimestampRangeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TSRANGE"), new FunctionArgument("b", "INTEGER") },
            ReturnType = "TSRANGE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-tsrange",
                "IncreaseTimestampRangeFSharp1",
                "'[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, 1",
                "= '[2021-01-02 14:30, 2021-01-02 15:30)'::TSRANGE"
            },
            new object[] { "f#-tsrange",
                "IncreaseTimestampRangeFSharp2",
                "'[, 2021-01-01 15:30)'::TSRANGE, 3",
                "= '[, 2021-01-04 15:30)'::TSRANGE"
            },
            new object[] { "f#-tsrange",
                "IncreaseTimestampRangeFSharp3",
                "'[,)'::TSRANGE, 3",
                "= '(,)'::TSRANGE"
            },
            new object[] { "f#-tsrange",
                "IncreaseTimestampRangeFSharp4",
                "'(2021-01-01 14:30, 2021-01-01 15:30]'::TSRANGE, 3",
                "= '(2021-01-04 14:30, 2021-01-04 15:30]'::TSRANGE"
            },
            new object[] {
                "f#-tsrange-null",
                "IncreaseTimestampRangeFSharp5",
                "NULL::TSRANGE, 3",
                @"= '[""2022-01-04 12:30:30"",""2022-12-28 17:30:30"")'::TSRANGE"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampRangeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
