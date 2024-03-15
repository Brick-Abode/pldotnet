
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class Modify2DArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
integers.SetValue(int64 new_value, 0, 0)
integers
    ";

    public Modify2DArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Modify2DArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "int8[]"), new FunctionArgument("new_value", "int2") },
            ReturnType = "int8[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8-null-2array", "modify2DArrayFSharp1", "ARRAY[[null::int8, null::int8], [2047483647::int8, 304325::int8]], '250'::int2", "= ARRAY[[250::int8, null::int8], [2047483647::int8, 304325::int8]]" },
        new object[] { "f#-int8-null-2array", "modify2DArrayFSharp2", "ARRAY[[2047483647::int8, 304325::int8], [null::int8, 12465464::int8]], '32767'::int2", "= ARRAY[[32767::int8, 304325::int8], [null::int8, 12465464::int8]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModify2DArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
