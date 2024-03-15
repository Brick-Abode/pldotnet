
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class IncreaseDateOnlyRangeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlRange<DateOnly>(new DateOnly(2022, 1, 1), true, false, new DateOnly(2022, 12, 25), false, false);

if (days_to_add == null)
    days_to_add = 1;

NpgsqlRange<DateOnly> non_null_value = (NpgsqlRange<DateOnly>)orig_value;

return new NpgsqlRange<DateOnly>(non_null_value.LowerBound.AddDays((int)days_to_add), non_null_value.LowerBoundIsInclusive, non_null_value.LowerBoundInfinite, non_null_value.UpperBound.AddDays((int)days_to_add), non_null_value.UpperBoundIsInclusive, non_null_value.UpperBoundInfinite);
    ";

    public IncreaseDateOnlyRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseDateOnlyRange",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "DATERANGE"), new FunctionArgument("days_to_add", "INTEGER") },
            ReturnType = "DATERANGE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-daterange", "IncreaseDateonlyRange1", "'[2021-01-01, 2021-01-04)'::DATERANGE, 1", "= '[2021-01-02, 2021-01-05)'::DATERANGE" },
        new object[] { "c#-daterange", "IncreaseDateonlyRange2", "'[, 2021-01-01)'::DATERANGE, 3", "= '[, 2021-01-04)'::DATERANGE" },
        new object[] { "c#-daterange", "IncreaseDateonlyRange3", "'[,)'::DATERANGE, 3", "= '(,)'::DATERANGE" },
        new object[] { "c#-daterange", "IncreaseDateonlyRange4", "'(2021-01-01, 2021-01-04]'::DATERANGE, 3", "= '(2021-01-04, 2021-01-07]'::DATERANGE" },
        new object[] { "c#-daterange-null", "IncreaseDateonlyRange5", "NULL::DATERANGE, 3", "= '[2022-01-04,2022-12-28)'::DATERANGE" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseDateOnlyRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
