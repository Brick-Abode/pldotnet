
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Date")]
public class CreateDateFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (year.HasValue, month.HasValue, day.HasValue) with
| (true, true, true) -> Nullable (new DateOnly(year.Value, month.Value, day.Value))
| _ -> System.Nullable()
    ";

    public CreateDateFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateDateFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("year", "int"), new FunctionArgument("month", "int"), new FunctionArgument("day", "int") },
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
            new object[] { "f#-date", "createDateFSharp", "CAST(2022 AS int), CAST(10 AS int), CAST(14 AS int)", "= DATE 'Oct-14-2022'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
