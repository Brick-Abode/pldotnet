
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class PathToTemporaryFilesFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.PathToTemporaryFiles
    ";

    public PathToTemporaryFilesFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "pathToTemporaryFilesFSharp",
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
                "f#-pldotnet-setting-pathToTemporaryFilesFSharp",
                "pathToTemporaryFilesFSharp",
                "",
                "= '/tmp/PlDotNET/'"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPathToTemporaryFilesFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
