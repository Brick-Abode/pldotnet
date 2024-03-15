
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MixedBigInT8Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (short)(b+c);
    ";

    public MixedBigInT8Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MixedBigInT8",
            Arguments = new List<FunctionArgument> { new FunctionArgument("b", "smallint"), new FunctionArgument("c", "bigint") },
            ReturnType = "smallint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8", "mixedBigInt8", "CAST(32 AS SMALLINT), CAST(100 AS BIGINT)", "= smallint '132'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMixedBigInT8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
