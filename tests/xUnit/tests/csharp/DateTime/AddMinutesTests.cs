
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class AddMinutesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_time == null) {
    orig_time = new TimeOnly(0, 30, 20);
}

TimeOnly new_time = ((TimeOnly)orig_time).AddMinutes((double) min_to_add);
return new_time;
    ";

    public AddMinutesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddMinutes",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_time", "TIME"), new FunctionArgument("min_to_add", "INT") },
            ReturnType = "TIME",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-time", "addMinutes1", "TIME '05:30 PM', 75", "= TIME '06:45 PM'" },
        new object[] { "c#-time-null", "addMinutes2", "NULL::TIME, 75", "= TIME '01:45:20'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddMinutes(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
