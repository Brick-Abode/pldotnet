
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class BooleanAndTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null) {
    a = false;
}

if (b == null) {
    b = false;
}

return a&b;
    ";

    public BooleanAndTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "BooleanAnd",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("a", "boolean"),
                new FunctionArgument("b", "boolean")
            },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }
    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bool", "BooleanAnd1", "true, true", "is true" },
            new object[] { "c#-bool-null", "BooleanAnd2", "NULL::BOOLEAN, true", "is false" }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanAnd(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
