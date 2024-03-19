
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class SetNewDateFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_timestamp = if orig_timestamp.HasValue then orig_timestamp.Value else DateTime(2022, 1, 1, 8, 30, 20)
let new_date = if new_date.HasValue then new_date.Value else DateOnly(2023, 12, 25)
let new_day = new_date.Day
let new_month = new_date.Month
let new_year = new_date.Year
DateTime(new_year, new_month, new_day, orig_timestamp.Hour, orig_timestamp.Minute, orig_timestamp.Second)
    ";

    public SetNewDateFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SetNewDateFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_timestamp", "TIMESTAMP"), new FunctionArgument("new_date", "DATE") },
            ReturnType = "TIMESTAMP",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamp", "setNewDateFSharp1", "TIMESTAMP '2004-10-19 10:23:54 PM', DATE '2022-10-17'", "= TIMESTAMP '2022-10-17 10:23:54 PM'" },
        new object[] { "f#-timestamp-null", "setNewDateFSharp2", "NULL::TIMESTAMP, NULL::DATE", "= TIMESTAMP '2023-12-25 08:30:20'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSetNewDateFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
