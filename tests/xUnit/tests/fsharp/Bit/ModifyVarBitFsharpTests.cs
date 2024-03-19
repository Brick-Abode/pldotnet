
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bit")]
public class ModifyVarBitFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if System.Object.ReferenceEquals(a, null) then
        null
    else
        let result = a
        result.[0] <- if a.[0] = false then true else false
        result.[a.Length - 1] <- if a.[a.Length - 1] = false then true else false
        result
    ";

    public ModifyVarBitFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyVarBitFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a BIT", "VARYING") },
            ReturnType = "BIT VARYING",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varbit", "modifyvarbitfsharp1", "'1001110001000'::BIT VARYING", "= '0001110001001'::BIT VARYING" },
        new object[] { "f#-varbit-null", "modifyvarbitfsharp2", "NULL::BIT VARYING", "IS NULL" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyVarBitFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
