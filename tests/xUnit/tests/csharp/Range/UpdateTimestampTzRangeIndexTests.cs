
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class UpdateTimestampTzRangeIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateTimestampTzRangeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateTimestampTzRangeIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TSTZRANGE[]"), new FunctionArgument("desired", "TSTZRANGE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-tstzrange-null-1array", "updateTimestampTzRangeIndex1", "ARRAY['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, null::TSTZRANGE, '[,)'::TSTZRANGE], '[2021-05-25 14:30 +03,)'::TSTZRANGE, ARRAY[2]", "= ARRAY['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE, '[2021-05-25 14:30 +03,)'::TSTZRANGE, '[,)'::TSTZRANGE]" },
        new object[] { "c#-tstzrange-null-2array", "updateTimestampTzRangeIndex2", "ARRAY[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE, ARRAY[1, 0]", "= ARRAY[['[2021-01-01 14:30 +02, 2021-01-01 15:30 -05)'::TSTZRANGE, '(, 2021-04-01 15:30 +05)'::TSTZRANGE], ['[2021-05-25 14:30 +03,)'::TSTZRANGE, '[,)'::TSTZRANGE]]" },
        new object[] { "c#-tstzrange-null-2array-arraynull", "updateTimestampTzRangeIndex3", "ARRAY[[null::TSTZRANGE, null::TSTZRANGE], [null::TSTZRANGE, '[,)'::TSTZRANGE]], '[2021-05-25 14:30 +03,)'::TSTZRANGE, ARRAY[1, 0]", "= ARRAY[[null::TSTZRANGE, null::TSTZRANGE], ['[2021-05-25 14:30 +03,)'::TSTZRANGE, '[,)'::TSTZRANGE]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateTimestampTzRangeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
