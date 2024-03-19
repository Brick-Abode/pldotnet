
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreaseCircleFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_value = if orig_value.HasValue then orig_value.Value else NpgsqlCircle(NpgsqlPoint(0, 0), 3)
NpgsqlCircle(orig_value.Center, (orig_value.Radius + 1.0))
    ";

    public IncreaseCircleFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseCircleFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "CIRCLE") },
            ReturnType = "CIRCLE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-circle-null", "increaseCircleFSharp1", "NULL::CIRCLE", "= CIRCLE '<(0, 0), 4>'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseCircleFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
