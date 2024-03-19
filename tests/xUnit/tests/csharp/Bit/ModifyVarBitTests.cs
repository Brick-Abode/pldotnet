
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class ModifyVarBitTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
        return null;

    a[0] = a[0] ? false : true;
    a[a.Length-1] = a[a.Length-1] ? false : true;
    return a;
    ";

    public ModifyVarBitTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyVarBit",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a BIT", "VARYING") },
            ReturnType = "BIT VARYING",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varbit", "modifyvarbit1", "'1001110001000'::BIT VARYING", "= '0001110001001'::BIT VARYING" },
        new object[] { "c#-varbit-null", "modifyvarbit2", "NULL::BIT VARYING", "IS NULL" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyVarBit(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
