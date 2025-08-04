
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class SaveSourceCodeFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.SaveSourceCode
    ";

    public SaveSourceCodeFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "saveSourceCodeFSharp",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "boolean",
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
                "f#-pldotnet-setting-saveSourceCodeFSharp",
                "saveSourceCodeFSharpFalse",
                "",
                "is true"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSaveSourceCodeFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
