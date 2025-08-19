using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class PrintSourceCodeTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return settings.PrintSourceCode;
    ";

    public PrintSourceCodeTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "printSourceCode",
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
            new object[] { "pldotnet-settings-printSourceCode", "printSourceCodeFalse", "", "is false" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSourceCode(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
