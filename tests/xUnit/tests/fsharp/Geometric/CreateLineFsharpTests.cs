
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateLineFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlLine(a, b, c)
    ";

    public CreateLineFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLineFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "float8"), new FunctionArgument("b", "float8"), new FunctionArgument("c", "float8") },
            ReturnType = "LINE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-line", "createLineFSharp", "1.50,-2.750,3.25", "= LINE '{1.50,-2.750,3.25}'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
