
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class SumRealFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else float32 0.0
let b = if b.HasValue then b.Value else float32 0.0
System.Nullable(a + b)
    ";

    public SumRealFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumRealFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "float4"), new FunctionArgument("b", "float4") },
            ReturnType = "float4",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4", "sumRealFSharp1", "1.50055, 1.50054", "= float4 '3.00109'" },
        new object[] { "f#-float4-null", "sumRealFSharp2", "NULL, 1.50054", "= float4 '1.50054'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumRealFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
