
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class ModifyInputDateFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_date = if System.Object.ReferenceEquals(orig_date, null) then DateOnly(2022, 1, 1) else orig_date.Value
let day = orig_date.Day
let month = orig_date.Month
let year = orig_date.Year
DateOnly(year + 3, month + 1, day + 6)
    ";

    public ModifyInputDateFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyInputDateFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_date", "DATE") },
            ReturnType = "DATE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-date", "modifyInputDateFSharp1", "DATE 'Oct-14-2022'", "= DATE 'Nov-20-2025'" },
        new object[] { "f#-date-null", "modifyInputDateFSharp2", "NULL::DATE", "= DATE 'Feb-07-2025'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyInputDateFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
