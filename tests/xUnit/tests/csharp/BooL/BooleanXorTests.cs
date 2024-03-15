
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class BooleanXorTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null) {
    a = false;
}

if (b == null) {
    b = false;
}

return a^b;
    ";

    public BooleanXorTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "BooleanXor",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") },
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
        new object[] { "c#-bool", "BooleanXor1", "false, false", "is false" },
        new object[] { "c#-bool-null", "BooleanXor2", "NULL::BOOLEAN, NULL::BOOLEAN", "is false" }
    };
}


    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanXor(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
