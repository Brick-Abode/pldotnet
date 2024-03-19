
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ConcatenateVarCharsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
        a = """";

    if (b == null)
        b = """";

    if (c == null)
        c = """";

    return (a + "" "" + b + "" "" + c).ToUpper();
    ";

    public ConcatenateVarCharsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateVarChars",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "VARCHAR"), new FunctionArgument("c", "BPCHAR") },
            ReturnType = "VARCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varchar", "concatenateVarChars1", "'hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::VARCHAR" },
        new object[] { "c#-varchar-null", "concatenateVarChars2", "NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::VARCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarChars(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
