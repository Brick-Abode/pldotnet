
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class IncreaseDateOnlyRangeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else NpgsqlRange<DateOnly>(DateOnly(2022, 1, 1), true, false, DateOnly(2022, 12, 25), false, false)
let b = if b.HasValue then b.Value else 1
NpgsqlRange<DateOnly>(a.LowerBound.AddDays(int b), a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound.AddDays(int b), a.UpperBoundIsInclusive, a.UpperBoundInfinite)
    ";

    public IncreaseDateOnlyRangeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseDateOnlyRangeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "DATERANGE"), new FunctionArgument("b", "INTEGER") },
            ReturnType = "DATERANGE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-daterange", "IncreaseDateonlyRangeFSharp1", "'[2021-01-01, 2021-01-04)'::DATERANGE, 1", "= '[2021-01-02, 2021-01-05)'::DATERANGE" },
        new object[] { "f#-daterange", "IncreaseDateonlyRangeFSharp2", "'[, 2021-01-01)'::DATERANGE, 3", "= '[, 2021-01-04)'::DATERANGE" },
        new object[] { "f#-daterange", "IncreaseDateonlyRangeFSharp3", "'[,)'::DATERANGE, 3", "= '(,)'::DATERANGE" },
        new object[] { "f#-daterange", "IncreaseDateonlyRangeFSharp4", "'(2021-01-01, 2021-01-04]'::DATERANGE, 3", "= '(2021-01-04, 2021-01-07]'::DATERANGE" },
        new object[] { "f#-daterange-null", "IncreaseDateonlyRangeFSharp5", "NULL::DATERANGE, 3", "= '[2022-01-04,2022-12-28)'::DATERANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseDateOnlyRangeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
