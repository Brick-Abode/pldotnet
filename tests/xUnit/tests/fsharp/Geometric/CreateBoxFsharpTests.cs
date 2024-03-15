
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateBoxFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlBox(high, low)
    ";

    public CreateBoxFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBoxFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") },
            ReturnType = "BOX",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-box", "createBoxFSharp", "POINT '(2.052787, 3.005716)', POINT '(0.025988, 1.021653)'", "= BOX '(2.052787, 3.005716), (0.025988, 1.021653)'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBoxFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
