
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateTimestampTzRangeArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateTime>(DateTime(2022, 4, 14, 12, 30, 25, DateTimeKind.Utc), true, false, DateTime(2022, 4, 15, 17, 30, 25, DateTimeKind.Utc), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateTimestampTzRangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampTzRangeArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TSTZRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-tstzrange-null-3array-arraynull", "CreateTimestampTzRangeArrayFSharp1", "", "= ARRAY[[['[2022-04-14 12:30:25 +00, 2022-04-15 17:30:25 +00)'::TSTZRANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampTzRangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
