
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ReturnPathTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return orig_path;
    ";

    public ReturnPathTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnPath",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_path", "PATH") },
            ReturnType = "PATH",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {"c#-path","returnPath - open","PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'"," <= PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'"},
            new object[] {"c#-path","returnPath - close","PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'","<= PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'"},
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnPath(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
