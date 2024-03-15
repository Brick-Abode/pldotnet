
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateTimestampTzMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let objects_value = DateTime(2022, 11, 15, 13, 23, 45, DateTimeKind.Utc);
let arr = Array.CreateInstance(typeof<DateTime>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateTimestampTzMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampTzMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TIMESTAMP WITH TIME ZONE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamptz-3array", "CreateTimestamptzMultidimensionalArrayFSharp", "", "= ARRAY[[[TIMESTAMP WITH TIME ZONE '2022-11-15 13:23:45 +00']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampTzMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
