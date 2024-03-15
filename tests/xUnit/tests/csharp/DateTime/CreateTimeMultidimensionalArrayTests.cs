
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateTimeMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int hour = 10;
int minute = 33;
int second = 55;
TimeOnly objects_value = new TimeOnly(hour,minute,second);
TimeOnly?[, ,] three_dimensional_array = new TimeOnly?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateTimeMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimeMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TIME[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-time-3array", "CreateTimeMultidimensionalArray", "", "= ARRAY[[[TIME '10:33:55 AM', TIME '10:33:55 AM'], [null::time, null::time]], [[TIME '10:33:55 AM', null::time], [TIME '10:33:55 AM', TIME '10:33:55 AM']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimeMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
