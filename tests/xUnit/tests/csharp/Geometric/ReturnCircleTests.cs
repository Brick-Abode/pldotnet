
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ReturnCircleTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return orig_circle;
    ";

    public ReturnCircleTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnCircle",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_circle", "CIRCLE") },
            ReturnType = "CIRCLE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {"c#-circle","returnCircle","CIRCLE '2.5, 3.5, 12.78'","~= CIRCLE '<(2.5, 3.5), 12.78>'"}
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnCircle(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
