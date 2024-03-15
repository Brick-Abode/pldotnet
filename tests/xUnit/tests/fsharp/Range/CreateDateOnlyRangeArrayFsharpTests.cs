
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateDateOnlyRangeArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<DateOnly>>, 1, 1, 1)
let objects_value = NpgsqlRange<DateOnly>(DateOnly(2022, 4, 14), true, false, DateOnly(2022, 4, 15), false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateDateOnlyRangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateDateOnlyRangeArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "DATERANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-daterange-null-3array-arraynull", "CreateDateonlyRangeArrayFSharp1", "", "= ARRAY[[['[2022-04-14, 2022-04-15)'::DATERANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateOnlyRangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
