
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MaxSmallInTTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return (short)32767;
    ";

    public MaxSmallInTTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MaxSmallInT",
            Arguments = new List<FunctionArgument> {  },
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
            new object[] { "c#-int2", "maxSmallInt", "", "= smallint '32767'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxSmallInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
