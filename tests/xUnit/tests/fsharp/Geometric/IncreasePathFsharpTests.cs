
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreasePathFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let orig_value = if orig_value.HasValue then orig_value.Value else NpgsqlPath(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100), NpgsqlPoint(200, 200))
let new_value = NpgsqlPath(orig_value.Count)
for polygon_point in orig_value do
    new_value.Add(NpgsqlPoint(polygon_point.X + 1.0, polygon_point.Y + 1.0));
new_value
    ";

    public IncreasePathFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreasePathFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "PATH") },
            ReturnType = "PATH",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-path", "increasePathFSharp1", "'((1,1),(101,101),(201,201))'::PATH", "= '((2,2),(102,102),(202,202))'::PATH" },
        new object[] { "f#-path-null", "increasePathFSharp2", "NULL::PATH", "= '((1,1),(101,101),(201,201))'::PATH" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreasePathFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
