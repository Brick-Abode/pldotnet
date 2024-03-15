
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateTimestampMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let objects_value = DateTime(2022, 11, 15, 13, 23, 45)
let arr = Array.CreateInstance(typeof<DateTime>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateTimestampMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TIMESTAMP[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamp-3array", "CreateTimestampMultidimensionalArrayFSharp", "", "= ARRAY[[[TIMESTAMP '2022-11-15 13:23:45']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
