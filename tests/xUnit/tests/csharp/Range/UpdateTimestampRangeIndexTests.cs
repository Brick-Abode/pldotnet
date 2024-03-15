
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class UpdateTimestampRangeIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateTimestampRangeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateTimestampRangeIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TSRANGE[]"), new FunctionArgument("desired", "TSRANGE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-tsrange-null-1array", "updateTimestampRangeIndex1", "ARRAY['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, null::TSRANGE, '[,)'::TSRANGE], '[2021-05-25 14:30,)'::TSRANGE, ARRAY[2]", "= ARRAY['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE, '[2021-05-25 14:30,)'::TSRANGE, '[,)'::TSRANGE]" },
        new object[] { "c#-tsrange-null-2array", "updateTimestampRangeIndex2", "ARRAY[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE, ARRAY[1, 0]", "= ARRAY[['[2021-01-01 14:30, 2021-01-01 15:30)'::TSRANGE, '(, 2021-04-01 15:30)'::TSRANGE], ['[2021-05-25 14:30,)'::TSRANGE, '[,)'::TSRANGE]]" },
        new object[] { "c#-tsrange-null-2array-arraynull", "updateTimestampRangeIndex3", "ARRAY[[null::TSRANGE, null::TSRANGE], [null::TSRANGE, '[,)'::TSRANGE]], '[2021-05-25 14:30,)'::TSRANGE, ARRAY[1, 0]", "= ARRAY[[null::TSRANGE, null::TSRANGE], ['[2021-05-25 14:30,)'::TSRANGE, '[,)'::TSRANGE]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateTimestampRangeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
