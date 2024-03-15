
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class CreateTimestampTzRangeArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlRange<DateTime> objects_value = new NpgsqlRange<DateTime>(new DateTime(2022, 4, 14, 12, 30, 25, DateTimeKind.Utc), true, false, new DateTime(2022, 4, 15, 17, 30, 25, DateTimeKind.Utc), false, false);
NpgsqlRange<DateTime>?[, ,] three_dimensional_array = new NpgsqlRange<DateTime>?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateTimestampTzRangeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampTzRangeArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TSTZRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-tstzrange-null-3array-arraynull", "CreateTimestampTzRangeArray1", "", "= ARRAY[[['[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE,'[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE], [null::TSTZRANGE, null::TSTZRANGE]], [['[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE, null::TSTZRANGE], ['[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE, '[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampTzRangeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
