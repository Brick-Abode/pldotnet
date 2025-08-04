
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class PrintSourceCodeFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.PrintSourceCode
    ";

    public PrintSourceCodeFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "printSourceCodeFSharp",
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
                "f#-pldotnet-setting-printSourceCodeFSharp",
                "printSourceCodeFSharpFalse",
                "",
                "is false"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSourceCodeFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
