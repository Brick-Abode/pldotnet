
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateLineMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<NpgsqlLine>, 3, 3, 1)
let objects_value = NpgsqlLine(2.4,8.2,-32.43)
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
    ";

    public CreateLineMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLineMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "LINE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
            CastFunctionAs = "TEXT"
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-line-3array",
                "CreateLineMultidimensionalArrayFSharp1",
                "",
                @"= '{{{""{2.4,8.2,-32.43}""},{""{0,0,0}""},{""{0,0,0}""}},{{""{0,0,0}""},{""{2.4,8.2,-32.43}""},{""{0,0,0}""}},{{""{0,0,0}""},{""{0,0,0}""},{""{2.4,8.2,-32.43}""}}}'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
