
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class Modify1DArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
integers.SetValue((int)new_value, 0)
integers
    ";

    public Modify1DArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Modify1DArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "int4[]"), new FunctionArgument("new_value", "int2") },
            ReturnType = "int4[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4-null-1array", "modify1DArrayFSharp1", "ARRAY[2047483647::int4, null::int4, 304325::int4, 4356::int4], '250'::int2", "= ARRAY[250::int4, null::int4, 304325::int4, 4356::int4]" },
        new object[] { "f#-int4-null-1array", "modify1DArrayFSharp2", "ARRAY[null::int4, null::int4, 2047483647::int4, 304325::int4], '32767'::int2", "= ARRAY[32767::int4, null::int4, 2047483647::int4, 304325::int4]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModify1DArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
