
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class ModifyIntervalFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_interval = if orig_interval.HasValue then orig_interval.Value else NpgsqlInterval(4, 25, 900000000)
NpgsqlInterval(orig_interval.Months + months_to_add.Value, orig_interval.Days + days_to_add.Value, orig_interval.Time)
    ";

    public ModifyIntervalFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyIntervalFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_interval", "INTERVAL"), new FunctionArgument("days_to_add", "INT"), new FunctionArgument("months_to_add", "INT") },
            ReturnType = "INTERVAL",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-interval", "modifyIntervalFSharp1", "INTERVAL '4 hours 5 minutes 6 seconds', 15, 20", "= INTERVAL '1 YEAR 8 MONTHS 15 DAYS 4 HOURS 5 MINUTES 6 SECONDS'" },
        new object[] { "f#-interval-null", "modifyIntervalFSharp2", "NULL::INTERVAL, 15, 20", "= INTERVAL '2 YEAR 40 DAYS 15 MINUTES'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIntervalFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
