
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateInT8RangeArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<int64>>, 1, 1, 1)
let objects_value = NpgsqlRange<int64>(64, true, false, 89, false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateInT8RangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateInT8RangeArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "INT8RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8range-null-3array-arraynull", "CreateInt8RangeArrayFSharp1", "", "= ARRAY[[['[64,89)'::INT8RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInT8RangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
