
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class SetNewDateTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_timestamp == null) {
    orig_timestamp = new DateTime(2022, 1, 1, 8, 30, 20);
}

if (new_date == null) {
    new_date = new DateOnly(2023, 12, 25);
}

int new_day = ((DateOnly)new_date).Day;
int new_month = ((DateOnly)new_date).Month;
int new_year = ((DateOnly)new_date).Year;
DateTime new_timestamp = new DateTime(new_year, new_month, new_day, ((DateTime)orig_timestamp).Hour, ((DateTime)orig_timestamp).Minute, ((DateTime)orig_timestamp).Second);
return new_timestamp;
    ";

    public SetNewDateTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SetNewDate",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_timestamp", "TIMESTAMP"), new FunctionArgument("new_date", "DATE") },
            ReturnType = "TIMESTAMP",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamp", "setNewDate1", "TIMESTAMP '2004-10-19 10:23:54 PM', DATE '2022-10-17'", "= TIMESTAMP '2022-10-17 10:23:54 PM'" },
        new object[] { "c#-timestamp-null", "setNewDate2", "NULL::TIMESTAMP, NULL::DATE", "= TIMESTAMP '2023-12-25 08:30:20'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSetNewDate(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
