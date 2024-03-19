
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateInT4RangeArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<int>>, 1, 1, 1)
let objects_value = NpgsqlRange<int>(64, true, false, 89, false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";

    public CreateInT4RangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateInT4RangeArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "INT4RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4range-null-3array-arraynull", "CreateInt4RangeArrayFSharp1", "", "= ARRAY[[['[64,89)'::INT4RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInT4RangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
