using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class PathToTemporaryFilesTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return settings.PathToTemporaryFiles;
    ";

    public PathToTemporaryFilesTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "pathToTemporaryFiles",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "pldotnet-settings-pathToTemporaryFiles", "pathToTemporaryFiles", "", "= '/tmp/PlDotNET/'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPathToTemporaryFiles(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
