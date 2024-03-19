
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class Mult2InTFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (a.HasValue, b.HasValue) with
    | (false, false) -> System.Nullable()
    | (true, false) -> Nullable(a.Value)
    | (false, true) -> Nullable(b.Value)
    | (true, true) -> Nullable (a.Value*b.Value)
    ";

    public Mult2InTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Mult2InTFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT4"), new FunctionArgument("b", "INT4") },
            ReturnType = "INT4",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4", "mult2IntFSharp1", "'25'::INT2, '30'::INT2", "= '750'::INT4" },
        new object[] { "f#-int4-null", "mult2IntFSharp2", "'25'::INT2, NULL::INT2", "= '25'::INT4" },
        new object[] { "f#-int4-null", "mult2IntFSharp3", "NULL::INT2, '30'::INT2", "= '30'::INT4" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMult2InTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
