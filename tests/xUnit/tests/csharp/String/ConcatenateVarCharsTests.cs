using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateVarCharsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateVarCharsTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ConcatenateVarChars", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "VARCHAR"), new FunctionArgument("c", "BPCHAR") }, ReturnType = "VARCHAR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varchar", "concatenateVarChars1", "'hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::VARCHAR" }, new object[] { "c#-varchar-null", "concatenateVarChars2", "NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::VARCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarChars(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ConcatenateVarCharsTestsCSharp : BaseConcatenateVarCharsTests
{
    protected override string FunctionBody => @"
if (a == null)
        a = """";

    if (b == null)
        b = """";

    if (c == null)
        c = """";

    return (a + "" "" + b + "" "" + c).ToUpper();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}