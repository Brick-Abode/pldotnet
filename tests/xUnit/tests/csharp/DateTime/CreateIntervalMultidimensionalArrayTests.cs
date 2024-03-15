
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateIntervalMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int months = 10;
int days = 33;
long time = 9000000000;
NpgsqlInterval objects_value = new NpgsqlInterval(months, days, time);
NpgsqlInterval?[, ,] three_dimensional_array = new NpgsqlInterval?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateIntervalMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateIntervalMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "INTERVAL[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-interval-3array", "CreateIntervalMultidimensionalArray", "", "= ARRAY[[[INTERVAL '10 months 33 days 2 hours 30 minutes', INTERVAL '10 months 33 days 2 hours 30 minutes'], [null::interval, null::interval]], [[INTERVAL '10 months 33 days 2 hours 30 minutes', null::interval], [INTERVAL '10 months 33 days 2 hours 30 minutes', INTERVAL '10 months 33 days 2 hours 30 minutes']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateIntervalMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
