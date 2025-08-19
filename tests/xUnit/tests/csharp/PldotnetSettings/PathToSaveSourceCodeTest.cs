using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class PathToSaveSourceCodeTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return settings.PathToSaveSourceCode;
    ";

    public PathToSaveSourceCodeTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "pathToSaveSourceCode",
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
            new object[] { "pldotnet-settings-pathToSaveSourceCode", "pathToSaveSourceCode", "", "= '/tmp/PlDotNET/GeneratedCodes'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPathToSaveSourceCode(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
