
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class ReturnSmallInTArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
small_integers
    ";

    public ReturnSmallInTArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnSmallInTArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("small_integers", "int2[]") },
            ReturnType = "int2[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int2-null-1array", "returnSmallIntArrayFSharp1", "ARRAY[12345::int2, null::int2, 123::int2, 4356::int2]", "= ARRAY[12345::int2, null::int2, 123::int2, 4356::int2]" },
        new object[] { "f#-int2-null-2array-arraynull", "returnSmallIntArrayFSharp2", "ARRAY[[null::int2, null::int2], [12345::int2, 654::int2]]", "= ARRAY[[null::int2, null::int2], [12345::int2, 654::int2]]" },
        new object[] { "f#-int2-null-3array-arraynull", "returnSmallIntArrayFSharp3", "ARRAY[[[null::int2, null::int2], [null::int2, null::int2]], [[186::int2, 23823::int2], [9521::int2, 934::int2]]]", "= ARRAY[[[null::int2, null::int2], [null::int2, null::int2]], [[186::int2, 23823::int2], [9521::int2, 934::int2]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnSmallInTArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
