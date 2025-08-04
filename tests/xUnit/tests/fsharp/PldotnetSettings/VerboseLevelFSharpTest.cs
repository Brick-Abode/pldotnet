
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class VerboseLevelFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.VerboseLevel
    ";

    public VerboseLevelFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "verboseLevelFSharp",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "int",
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
                "f#-pldotnet-setting-verboseLevelFSharp",
                "verboseLevelFSharp",
                "",
                "= 0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestVerboseLevelFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
