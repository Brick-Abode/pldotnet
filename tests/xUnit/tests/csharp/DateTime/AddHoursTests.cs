
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class AddHoursTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_time == null) {
    orig_time = new DateTimeOffset(2022, 1, 1, 8, 30, 20, new TimeSpan(2, 0, 0));
}

return ((DateTimeOffset)orig_time).AddHours((double)hours_to_add);
    ";

    public AddHoursTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddHours",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_time", "TIMETZ"), new FunctionArgument("hours_to_add", "FLOAT") },
            ReturnType = "TIMETZ",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timetz", "addHours1", "TIMETZ '04:05:06-08:00',1.5", "= TIMETZ '05:35:06-08:00'" },
        new object[] { "c#-timetz-null", "addHours2", "NULL::TIMETZ,1.5", "= TIMETZ '10:00:20+02:00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddHours(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
