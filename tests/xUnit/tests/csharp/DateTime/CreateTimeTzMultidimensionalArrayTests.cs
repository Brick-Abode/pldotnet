
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateTimeTzMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int hour = 10;
int minute = 33;
int second = 55;
DateTimeOffset objects_value = new DateTimeOffset(2022, 12, 25, hour, minute, second, new TimeSpan(2, 0, 0));
DateTimeOffset?[, ,] three_dimensional_array = new DateTimeOffset?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateTimeTzMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimeTzMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TIMETZ[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timetz-3array", "CreateTimetzMultidimensionalArray", "", "= ARRAY[[[TIMETZ '10:33:55+02:00', TIMETZ '10:33:55+02:00'], [null::timetz, null::timetz]], [[TIMETZ '10:33:55+02:00', null::timetz], [TIMETZ '10:33:55+02:00', TIMETZ '10:33:55+02:00']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimeTzMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
