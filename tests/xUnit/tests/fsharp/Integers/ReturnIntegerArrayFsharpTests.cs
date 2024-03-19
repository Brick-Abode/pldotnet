
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class ReturnIntegerArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
integers
    ";

    public ReturnIntegerArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnIntegerArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "int4[]") },
            ReturnType = "int4[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4-null-1array", "returnIntegerArrayFSharp1", "ARRAY[12345::int4, null::int4, 123::int4, 4356::int4]", "= ARRAY[12345::int4, null::int4, 123::int4, 4356::int4]" },
        new object[] { "f#-int4-null-2array-arraynull", "returnIntegerArrayFSharp2", "ARRAY[[null::int4, null::int4], [12345::int4, 654::int4]]", "= ARRAY[[null::int4, null::int4], [12345::int4, 654::int4]]" },
        new object[] { "f#-int4-null-3array-arraynull", "returnIntegerArrayFSharp3", "ARRAY[[[null::int4, null::int4], [null::int4, null::int4]], [[186::int4, 23823::int4], [9521::int4, 934::int4]]]", "= ARRAY[[[null::int4, null::int4], [null::int4, null::int4]], [[186::int4, 23823::int4], [9521::int4, 934::int4]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnIntegerArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
