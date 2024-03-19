
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateTimestampRangeArrayEmptyFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array.CreateInstance(typeof<NpgsqlRange<DateTime>>, 1, 1, 1)
    ";

    public CreateTimestampRangeArrayEmptyFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTimestampRangeArrayEmptyFSharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "TSRANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-tsrange-null-3array-arraynull", "CreateTimestampRangeArrayEmptyFSharp1", "", "= '{{{empty}}}'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimestampRangeArrayEmptyFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
