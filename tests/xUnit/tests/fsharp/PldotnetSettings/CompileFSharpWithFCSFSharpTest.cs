
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "PlDotNETSettings")]
public class CompileFSharpWithFCSFSharpTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let settings = PlDotNETSettings()
settings.CompileFSharpWithFCS
    ";

    public CompileFSharpWithFCSFSharpTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "compileFSharpWithFCS",
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
                "f#-pldotnet-setting-compileFSharpWithFCSFSharp",
                "compileFSharpWithFCSFSharpFalse",
                "",
                "is false"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompileFSharpWithFCSFSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
