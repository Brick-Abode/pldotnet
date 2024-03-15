
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class IncreaseMonthDateArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_dates = Array.CreateInstance(typeof(object), dates.Length);
ArrayManipulation.FlatArray(dates, ref flatten_dates);
for(int i = 0; i < flatten_dates.Length; i++)
{
    if (flatten_dates.GetValue(i) == null)
        continue;

    DateOnly orig_date = (DateOnly)flatten_dates.GetValue(i);
    int day = orig_date.Day;
    int month = orig_date.Month;
    int year = orig_date.Year;
    DateOnly new_date = new DateOnly(year,month+1,day);

    flatten_dates.SetValue((DateOnly)new_date, i);
}
return flatten_dates;
    ";

    public IncreaseMonthDateArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMonthDateArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("dates", "DATE[]") },
            ReturnType = "DATE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-date-1array", "IncreaseMonthDateArray1", "ARRAY[DATE 'Oct-14-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022']", "= ARRAY[DATE 'Nov-14-2022', DATE 'Nov-15-2022', null::date, DATE 'Nov-16-2022']" },
        new object[] { "c#-date-2array", "IncreaseMonthDateArray2", "ARRAY[[DATE 'Oct-14-2022', DATE 'Jan-15-2022'], [DATE 'Nov-18-2022', null::date]]", "= ARRAY[DATE 'Nov-14-2022', DATE 'Feb-15-2022', DATE 'Dec-18-2022', null::date]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMonthDateArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
