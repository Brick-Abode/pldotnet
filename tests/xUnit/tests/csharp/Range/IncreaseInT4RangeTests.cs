
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseInT4RangeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlRange<int>(0, true, false, 100, false, false);

if (increment_value == null)
    increment_value = 1;

NpgsqlRange<int> non_null_value = (NpgsqlRange<int>)orig_value;

return new NpgsqlRange<int>(non_null_value.LowerBound + (int)increment_value, non_null_value.LowerBoundIsInclusive, non_null_value.LowerBoundInfinite, non_null_value.UpperBound + (int)increment_value, non_null_value.UpperBoundIsInclusive, non_null_value.UpperBoundInfinite);
    ";

    public IncreaseInT4RangeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT4Range",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "INT4RANGE"), new FunctionArgument("increment_value", "INTEGER") },
            ReturnType = "INT4RANGE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4range", "IncreaseInt4Range1", "'[2,6)'::INT4RANGE, 1", "= '[3,7)'::INT4RANGE" },
        new object[] { "c#-int4range", "IncreaseInt4Range2", "'[,87)'::INT4RANGE, 3", "= '(,90)'::INT4RANGE" },
        new object[] { "c#-int4range", "IncreaseInt4Range3", "'[,)'::INT4RANGE, 3", "= '(,)'::INT4RANGE" },
        new object[] { "c#-int4range", "IncreaseInt4Range4", "'(-2147483648,2147483644)'::INT4RANGE, 3", "= '[-2147483644,2147483647)'::INT4RANGE" },
        new object[] { "c#-int4range", "IncreaseInt4Range5", "'(-456,-123]'::INT4RANGE, 1", "= '[-454,-121)'::INT4RANGE" },
        new object[] { "c#-int4range-null", "IncreaseInt4Range6", "NULL::INT4RANGE, 1", "= '[1,101)'::INT4RANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT4Range(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
