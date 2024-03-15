
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class ModifyCoefficientsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    if original_line.HasValue then
        let a = original_line.Value.A * -1.0
        let b = original_line.Value.B * -1.0
        let c = original_line.Value.C * -1.0
        NpgsqlLine(a, b, c)
    else
        NpgsqlLine(0,0,0)
    ";

    public ModifyCoefficientsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyCoefficientsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("original_line", "LINE") },
            ReturnType = "LINE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-line", "modifyCoefficientsFSharp1", "LINE '{-1.5,2.75,-3.25}'", "= LINE '{1.50,-2.75,3.25}'" },
            new object[] { "f#-line-null", "modifyCoefficientsFSharp2", "NULL::LINE", "= LINE '{2.4, 8.2, -32.43}'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyCoefficientsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
