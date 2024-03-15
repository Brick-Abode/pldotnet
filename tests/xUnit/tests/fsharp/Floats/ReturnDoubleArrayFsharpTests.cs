
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class ReturnDoubleArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
doubles
    ";

    public ReturnDoubleArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnDoubleArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("doubles", "float8[]") },
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
            new object[] { "f#-float8-null-1array", "returnDoubleArrayFSharp1", "ARRAY[21.0000000000109::float8, null::float8, 4.521234313421::float8, 7.412344328978::float8]", "= ARRAY[21.0000000000109::float8, null::float8, 4.521234313421::float8, 7.412344328978::float8]" },
        new object[] { "f#-float8-null-2array-arraynull", "returnDoubleArrayFSharp2", "ARRAY[[null::float8, null::float8], [4.521234313421::float8, 7.412344328978::float8]]", "= ARRAY[[null::float8, null::float8], [4.521234313421::float8, 7.412344328978::float8]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnDoubleArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
