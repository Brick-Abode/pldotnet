
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class CreateTimestampRangeArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlRange<DateTime> objects_value = new NpgsqlRange<DateTime>(new DateTime(2022, 4, 14, 12, 30, 25), true, false, new DateTime(2022, 4, 15, 17, 30, 25), false, false);
NpgsqlRange<DateTime>?[, ,] three_dimensional_array = new NpgsqlRange<DateTime>?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateTimestampRangeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampRangeArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TSRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-tsrange-null-3array-arraynull", "CreateTimestampRangeArray1", "", "= ARRAY[[['[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE,'[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE], [null::TSRANGE, null::TSRANGE]], [['[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE, null::TSRANGE], ['[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE, '[2022-04-14 12:30:25, 2022-04-15 17:30:25)'::TSRANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampRangeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
