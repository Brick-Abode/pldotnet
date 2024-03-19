
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseTimestampTzRangeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlRange<DateTime>(new DateTime(2022, 1, 1, 12, 30, 30, DateTimeKind.Utc), true, false, new DateTime(2022, 12, 25, 17, 30, 30, DateTimeKind.Utc), false, false);

if (days_to_add == null)
    days_to_add = 1;

NpgsqlRange<DateTime> non_null_value = (NpgsqlRange<DateTime>)orig_value;

return new NpgsqlRange<DateTime>(non_null_value.LowerBound.AddDays((int)days_to_add), non_null_value.LowerBoundIsInclusive, non_null_value.LowerBoundInfinite, non_null_value.UpperBound.AddDays((int)days_to_add), non_null_value.UpperBoundIsInclusive, non_null_value.UpperBoundInfinite);
    ";

    public IncreaseTimestampTzRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseTimestampTzRange",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "TSTZRANGE"), new FunctionArgument("days_to_add", "INTEGER") },
            ReturnType = "TSTZRANGE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-tstzrange", "IncreaseTimestampTzRange1", "'[2021-01-01 14:30 -03, 2021-01-04 15:30 +05)'::TSTZRANGE, 1", "= '[2021-01-02 14:30 -03, 2021-01-05 15:30 +05)'::TSTZRANGE" },
        new object[] { "c#-tstzrange", "IncreaseTimestampTzRange2", "'[, 2021-01-01 15:30 -03)'::TSTZRANGE, 3", "= '[, 2021-01-04 15:30 -03)'::TSTZRANGE" },
        new object[] { "c#-tstzrange", "IncreaseTimestampTzRange3", "'[,)'::TSTZRANGE, 3", "= '(,)'::TSTZRANGE" },
        new object[] { "c#-tstzrange", "IncreaseTimestampTzRange4", "'(2021-01-01 14:30 -03, 2021-01-04 15:30 +05]'::TSTZRANGE, 3", "= '(2021-01-04 14:30 -03, 2021-01-07 15:30 +05]'::TSTZRANGE" },
new object[] { "c#-tstzrange-null", "IncreaseTimestampTzRange5", "NULL::TSTZRANGE, 3", "= '[\"2022-01-04 12:30:30+00\",\"2022-12-28 17:30:30+00\")'::TSTZRANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseTimestampTzRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
