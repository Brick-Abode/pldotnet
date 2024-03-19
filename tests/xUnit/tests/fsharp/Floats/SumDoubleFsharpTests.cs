
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class SumDoubleFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else float 0.0
let b = if b.HasValue then b.Value else float 0.0
System.Nullable(a + b)
    ";

    public SumDoubleFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumDoubleFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "float8"), new FunctionArgument("b", "float8") },
            ReturnType = "float8",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float8", "sumDoubleFSharp1", "10.5000000000055, 10.5000000000054", "= float8  '21.0000000000109'" },
        new object[] { "f#-float8-null", "sumDoubleFSharp2", "NULL, NULL", "= float8 '0'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumDoubleFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
