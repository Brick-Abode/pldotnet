
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ConcatenateCharsTests : PlDotNetTest
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

    public ConcatenateCharsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateChars",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR"), new FunctionArgument("b", "BPCHAR"), new FunctionArgument("c", "BPCHAR") },
            ReturnType = "BPCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bpchar", "concatenateChars1", "'hello'::BPCHAR, 'beautiful'::BPCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::BPCHAR" },
        new object[] { "c#-bpchar-null", "concatenateChars2", "NULL::BPCHAR, 'beautiful'::BPCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::BPCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateChars(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
