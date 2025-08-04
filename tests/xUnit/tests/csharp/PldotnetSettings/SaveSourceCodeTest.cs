using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class SaveSourceCodeTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return settings.SaveSourceCode;
    ";

    public SaveSourceCodeTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "saveSourceCode",
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
            new object[] { "pldotnet-settings-saveSourceCode", "saveSourceCodeTrue", "", "is true" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSaveSourceCode(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
