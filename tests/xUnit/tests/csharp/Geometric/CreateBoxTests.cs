
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateBoxTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return new NpgsqlBox(high, low);
    ";

    public CreateBoxTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBox",
            Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") },
            ReturnType = "BOX",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-box", "createBox", "POINT '(2.052787, 3.005716)', POINT '(0.025988, 1.021653)'", "= BOX '(2.052787, 3.005716), (0.025988, 1.021653)'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBox(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
