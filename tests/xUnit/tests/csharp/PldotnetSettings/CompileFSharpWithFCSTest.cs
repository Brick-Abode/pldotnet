using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class CompileFSharpWithFCSTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return settings.CompileFSharpWithFCS;
    ";

    public CompileFSharpWithFCSTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "compileFSharpWithFCS",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "pldotnet-settings-compileFSharpWithFCS", "compileFSharpWithFCSFalse", "", "is false" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCompileFSharpWithFCS(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
