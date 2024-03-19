
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampRangeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlRange<DateTime>(new DateTime(2022, 1, 1, 12, 30, 30), true, false, new DateTime(2022, 12, 25, 17, 30, 30), false, false);

if (days_to_add == null)
    days_to_add = 1;

NpgsqlRange<DateTime> non_null_value = (NpgsqlRange<DateTime>)orig_value;

return new NpgsqlRange<DateTime>(non_null_value.LowerBound.AddDays((int)days_to_add), non_null_value.LowerBoundIsInclusive, non_null_value.LowerBoundInfinite, non_null_value.UpperBound.AddDays((int)days_to_add), non_null_value.UpperBoundIsInclusive, non_null_value.UpperBoundInfinite);
    ";

    public IncreaseTimestampRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseTimestampRange",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "TSRANGE"), new FunctionArgument("days_to_add", "INTEGER") },
            ReturnType = "TSRANGE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-tsrange", "IncreaseTimestampRange1", "'[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, 1", "= '[2021-01-02 14:30, 2021-01-02 15:30)'::TSRANGE" },
        new object[] { "c#-tsrange", "IncreaseTimestampRange2", "'[, 2021-01-01 15:30)'::TSRANGE, 3", "= '[, 2021-01-04 15:30)'::TSRANGE" },
        new object[] { "c#-tsrange", "IncreaseTimestampRange3", "'[,)'::TSRANGE, 3", "= '(,)'::TSRANGE" },
        new object[] { "c#-tsrange", "IncreaseTimestampRange4", "'(2021-01-01 14:30, 2021-01-01 15:30]'::TSRANGE, 3", "= '(2021-01-04 14:30, 2021-01-04 15:30]'::TSRANGE" },
new object[] { "c#-tsrange-null", "IncreaseTimestampRange5", "NULL::TSRANGE, 3", "= '[\"2022-01-04 12:30:30\",\"2022-12-28 17:30:30\")'::TSRANGE" }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
