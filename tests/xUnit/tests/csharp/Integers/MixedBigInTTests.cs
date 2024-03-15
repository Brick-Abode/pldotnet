
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MixedBigInTTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (long)a+(long)b+c;
    ";

    public MixedBigInTTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MixedBigInT",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer"), new FunctionArgument("c", "bigint") },
            ReturnType = "bigint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8", "mixedBigInt", "32767,  2147483647, 100", "= bigint '2147516514'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
