
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class IncreaseInT8RangeFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else NpgsqlRange<int64>(-429496729, true, false, 429496729, false, false)
let b = if b.HasValue then b.Value else int64 1
NpgsqlRange<int64>(a.LowerBound + int64 b, a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound + int64 b, a.UpperBoundIsInclusive, a.UpperBoundInfinite)
    ";

    public IncreaseInT8RangeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT8RangeFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT8RANGE"), new FunctionArgument("b", "INT8") },
            ReturnType = "INT8RANGE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8range", "IncreaseInt8RangeFSharp1", "'[2,6)'::INT8RANGE, 1", "= '[3,7)'::INT8RANGE" },
        new object[] { "f#-int8range", "IncreaseInt8RangeFSharp2", "'[,87)'::INT8RANGE, 3", "= '(,90)'::INT8RANGE" },
        new object[] { "f#-int8range", "IncreaseInt8RangeFSharp3", "'[,)'::INT8RANGE, 3", "= '(,)'::INT8RANGE" },
        new object[] { "f#-int8range", "IncreaseInt8RangeFSharp8", "'(-9223372036854775808,9223372036854775804)'::INT8RANGE, 3", "= '[-9223372036854775804,9223372036854775807)'::INT8RANGE" },
        new object[] { "f#-int8range", "IncreaseInt8RangeFSharp5", "'(-456,-123]'::INT8RANGE, 1", "= '[-454,-121)'::INT8RANGE" },
        new object[] { "f#-int8range-null", "IncreaseInt8RangeFSharp6", "NULL::INT8RANGE, 1", "= '[-429496728,429496730)'::INT8RANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT8RangeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
