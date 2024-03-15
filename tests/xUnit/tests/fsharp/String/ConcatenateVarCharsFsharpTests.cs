
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ConcatenateVarCharsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
(a + "" "" + b + "" "" + c).ToUpper()
    ";

    public ConcatenateVarCharsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateVarCharsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "VARCHAR"), new FunctionArgument("c", "BPCHAR") },
            ReturnType = "VARCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varchar", "concatenateVarCharsFSharp1", "'hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::VARCHAR" },
        new object[] { "f#-varchar-null", "concatenateVarCharsFSharp2", "NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::VARCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarCharsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
