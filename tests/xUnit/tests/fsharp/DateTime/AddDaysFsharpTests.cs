
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class AddDaysFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let my_timestamp = if my_timestamp.HasValue then my_timestamp.Value else DateTime(2022, 1, 1, 8, 30, 20, DateTimeKind.Utc)
my_timestamp.AddDays(int32 days_to_add)
    ";

    public AddDaysFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddDaysFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_timestamp TIMESTAMP WITH TIME", "ZONE"), new FunctionArgument("days_to_add", "INT") },
            ReturnType = "TIMESTAMP WITH TIME ZONE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamptz", "addDaysFSharp1", "TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', 2", "= TIMESTAMP WITH TIME ZONE '2004-10-21 22:23:54 +02'" },
        new object[] { "f#-timestamptz-null", "addDaysFSharp2", "NULL::TIMESTAMP WITH TIME ZONE, 2", "= TIMESTAMP WITH TIME ZONE '2022-01-03 08:30:20 +00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddDaysFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
