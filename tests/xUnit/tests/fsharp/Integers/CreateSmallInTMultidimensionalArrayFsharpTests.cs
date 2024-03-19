
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class CreateSmallInTMultidimensionalArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arr = Array.CreateInstance(typeof<int16>, 3, 3)
arr.SetValue((int16)1, 0, 0)
arr.SetValue((int16)1, 1, 1)
arr.SetValue((int16)1, 2, 2)
arr
    ";

    public CreateSmallInTMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateSmallInTMultidimensionalArrayFsharp",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "int2[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int2-2array", "CreateSmallIntMultidimensionalArrayFSharp", "", "= ARRAY[['1'::int2,'0'::int2,'0'::int2], ['0'::int2, '1'::int2, '0'::int2], ['0'::int2, '0'::int2, '1'::int2]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateSmallInTMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
