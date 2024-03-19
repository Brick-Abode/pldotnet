
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseInT8RangeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlRange<long>(-9223372036854775808, true, false, 9223372036854775804, false, false);

if (increment_value == null)
    increment_value = 1;

NpgsqlRange<long> non_null_value = (NpgsqlRange<long>)orig_value;

return new NpgsqlRange<long>(non_null_value.LowerBound + (int)increment_value, non_null_value.LowerBoundIsInclusive, non_null_value.LowerBoundInfinite, non_null_value.UpperBound + (int)increment_value, non_null_value.UpperBoundIsInclusive, non_null_value.UpperBoundInfinite);
    ";

    public IncreaseInT8RangeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT8Range",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "INT8RANGE"), new FunctionArgument("increment_value", "integer") },
            ReturnType = "INT8RANGE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8range", "IncreaseInt8Range1", "'[2,6)'::INT8RANGE, 1", "= '[3,7)'::INT8RANGE" },
        new object[] { "c#-int8range", "IncreaseInt8Range2", "'[,87)'::INT8RANGE, 3", "= '(,90)'::INT8RANGE" },
        new object[] { "c#-int8range", "IncreaseInt8Range3", "'[,)'::INT8RANGE, 3", "= '(,)'::INT8RANGE" },
        new object[] { "c#-int8range", "IncreaseInt8Range8", "'(-9223372036854775808,9223372036854775804)'::INT8RANGE, 3", "= '[-9223372036854775804,9223372036854775807)'::INT8RANGE" },
        new object[] { "c#-int8range", "IncreaseInt8Range5", "'(-456,-123]'::INT8RANGE, 1", "= '[-454,-121)'::INT8RANGE" },
        new object[] { "c#-int8range-null", "IncreaseInt8Range6", "NULL::INT8RANGE, 1", "= '[-9223372036854775807,9223372036854775805)'::INT8RANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT8Range(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
