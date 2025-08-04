using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "PlDotNETSettings")]

public class VerboseLevelTest : PlDotNetTest
{
    private static readonly string FunctionBody = @"
        var settings = new PlDotNETSettings();
        return Convert.ToInt32(settings.VerboseLevel);
    ";

    public VerboseLevelTest()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "verboseLevel",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "integer",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "pldotnet-settings-verboseLevel", "verboseLevelTrue", "", "= INTEGER '0'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestVerboseLevel(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
