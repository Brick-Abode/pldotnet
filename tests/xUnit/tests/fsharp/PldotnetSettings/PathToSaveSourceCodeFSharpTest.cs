
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class PathToSaveSourceCodeFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.PathToSaveSourceCode
    ";

    public PathToSaveSourceCodeFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "pathToSaveSourceCodeFSharp",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[]
            {
                "f#-pldotnet-setting-pathToSaveSourceCodeFSharp",
                "pathToSaveSourceCodeFSharp",
                "",
                "= '/tmp/PlDotNET/GeneratedCodes'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPathToSaveSourceCodeFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
