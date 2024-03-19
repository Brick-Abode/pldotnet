
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class ReturnBoolTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return false;
    ";

    public ReturnBoolTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnBool",
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
            new object[] { "c#-bool", "returnBool", "", " is false" },
        };
    }


    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBool(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
