
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class ConcatenateVarBitTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    BitArray c = new BitArray(a.Length+b.Length);
    for(int i = 0; i < a.Length;i++)
        c[i] = a[i];
    for(int i = 0, cont = a.Length; i < b.Length;i++)
        c[cont++] = b[i];
    return c;
    ";

    public ConcatenateVarBitTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateVarBit",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("a BIT", "VARYING"),
                new FunctionArgument("b BIT", "VARYING")
            },
            ReturnType = "BIT VARYING",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varbit", "concatenatevarbit1", "'1001110001000'::BIT VARYING, '111010111101111000'::BIT VARYING", "= '1001110001000111010111101111000'::BIT VARYING" },
            new object[] { "c#-varbit", "concatenatevarbit2", "'1001110001000'::BIT(10), '111010111101111000'::BIT VARYING", "= '1001110001111010111101111000'::BIT VARYING" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarBit(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
