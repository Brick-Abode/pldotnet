
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class CreateBooleanMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<bool>, 3, 3)
arr.SetValue(true, 0, 0)
arr.SetValue(true, 1, 1)
arr.SetValue(true, 2, 2)
arr
    ";

    public CreateBooleanMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBooleanMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "boolean[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bool-null-3array-arraynull", "CreateBooleanMultidimensionalArrayFSharp", "", "= ARRAY[[true, false, false], [false, true, false], [false, false, true]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBooleanMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
