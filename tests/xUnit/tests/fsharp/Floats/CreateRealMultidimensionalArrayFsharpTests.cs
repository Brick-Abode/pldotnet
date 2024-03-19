
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class CreateRealMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<float32>, 3, 3)
arr.SetValue(float32 1.24323, 0, 0)
arr.SetValue(float32 8.11134, 1, 1)
arr.SetValue(float32 16.14256, 2, 2)
arr
    ";

    public CreateRealMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateRealMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "real[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4-null-3array-arraynull", "CreateRealMultidimensionalArrayFSharp", "", "= ARRAY[[1.24323::real, 0::real, 0::real], [0::real, 8.11134::real, 0::real], [0::real, 0::real, 16.14256::real]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateRealMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
