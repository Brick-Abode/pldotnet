
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateTimestampTzMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int year = 2022;
int month = 11;
int day = 15;
int hour = 13;
int minute = 23;
int seconds = 45;
DateTime objects_value = new DateTime(year, month, day, hour, minute, seconds, DateTimeKind.Utc);
DateTime?[, ,] three_dimensional_array = new DateTime?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateTimestampTzMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampTzMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TIMESTAMP WITH TIME ZONE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamptz-3array", "CreateTimestamptzMultidimensionalArray", "", "= ARRAY[[[TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00', TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00'], [null::timestamp, null::timestamp]], [[TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00', null::timestamp], [TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00', TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampTzMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
