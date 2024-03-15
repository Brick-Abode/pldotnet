
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MixedInTTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (int)a+(int)b+c;
    ";

    public MixedInTTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MixedInT",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "smallint"), new FunctionArgument("b", "smallint"), new FunctionArgument("c", "integer") },
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
            new object[] { "c#-int4", "mixedInt", "CAST(32767 AS smallint),  CAST(32767 AS smallint), 100", "= integer '65634'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
