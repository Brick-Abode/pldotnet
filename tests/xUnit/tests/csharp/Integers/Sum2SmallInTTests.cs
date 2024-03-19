
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class Sum2SmallInTTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return (short)(a+b); //C# requires short cast
    ";

    public Sum2SmallInTTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2SmallInT",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "smallint"), new FunctionArgument("b", "smallint") },
            ReturnType = "smallint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int2", "sum2SmallInt", "CAST(100 AS smallint), CAST(101 AS smallint)", "= smallint '201'" },
        new object[] { "c#-int2-null", "sum2SmallInt2", "NULL::SMALLINT, 30::SMALLINT", "= smallint '30'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2SmallInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
