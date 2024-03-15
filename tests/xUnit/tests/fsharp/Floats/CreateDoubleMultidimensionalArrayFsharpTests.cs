
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class CreateDoubleMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<float>, 3, 3)
arr.SetValue(float 1.24323, 0, 0)
arr.SetValue(float 8.11134, 1, 1)
arr.SetValue(float 16.14256, 2, 2)
arr
    ";

    public CreateDoubleMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateDoubleMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "float8[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4-null-3array-arraynull", "CreateDoubleMultidimensionalArrayFSharp", "", "= ARRAY[[1.24323::float8, 0::float8, 0::float8], [0::float8, 8.11134::float8, 0::float8], [0::float8, 0::float8, 16.14256::float8]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDoubleMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
