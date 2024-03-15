
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class AddHoursFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_time = if orig_time.HasValue then orig_time.Value else DateTimeOffset(2022, 1, 1, 8, 30, 20, TimeSpan(2, 0, 0))
orig_time.AddHours(hours_to_add.Value)
    ";

    public AddHoursFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddHoursFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_time", "TIMETZ"), new FunctionArgument("hours_to_add", "FLOAT") },
            ReturnType = "TIMETZ",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timetz", "addHoursFSharp1", "TIMETZ '04:05:06-08:00',1.5", "= TIMETZ '05:35:06-08:00'" },
        new object[] { "f#-timetz-null", "addHoursFSharp2", "NULL::TIMETZ,1.5", "= TIMETZ '10:00:20+02:00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddHoursFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
