using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseInT4RangeFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseInT4RangeFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseInT4RangeFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT4RANGE"), new FunctionArgument("b", "INTEGER") }, ReturnType = "INT4RANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int4range", "IncreaseInt4RangeFSharp1", "'[2,6)'::INT4RANGE, 1", "= '[3,7)'::INT4RANGE" }, new object[] { "f#-int4range", "IncreaseInt4RangeFSharp2", "'[,87)'::INT4RANGE, 3", "= '(,90)'::INT4RANGE" }, new object[] { "f#-int4range", "IncreaseInt4RangeFSharp3", "'[,)'::INT4RANGE, 3", "= '(,)'::INT4RANGE" }, new object[] { "f#-int4range", "IncreaseInt4RangeFSharp4", "'(-2147483648,2147483644)'::INT4RANGE, 3", "= '[-2147483644,2147483647)'::INT4RANGE" }, new object[] { "f#-int4range", "IncreaseInt4RangeFSharp5", "'(-456,-123]'::INT4RANGE, 1", "= '[-454,-121)'::INT4RANGE" }, new object[] { "f#-int4range-null", "IncreaseInt4RangeFSharp6", "NULL::INT4RANGE, 1", "= '[1,101)'::INT4RANGE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT4RangeFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class IncreaseInT4RangeFsharpTestsFSharp : BaseIncreaseInT4RangeFsharpTests
{
    protected override string FunctionBody => @"
let a = if a.HasValue then a.Value else NpgsqlRange<int>(0, true, false, 100, false, false)
let b = if b.HasValue then b.Value else 1

NpgsqlRange<int>(a.LowerBound + int b, a.LowerBoundIsInclusive, a.LowerBoundInfinite, a.UpperBound + int b, a.UpperBoundIsInclusive, a.UpperBoundInfinite)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}