using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMultiplyVarCharTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMultiplyVarCharTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MultiplyVarChar", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "int") }, ReturnType = "VARCHAR", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varchar", "multiplyVarChar", "'hello '::VARCHAR, 5", "= 'HELLO HELLO HELLO HELLO HELLO '::VARCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyVarChar(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class MultiplyVarCharTestsCSharp : BaseMultiplyVarCharTests
{
    protected override string FunctionBody => @"
string c = """";
    for(int i=0;i<b;i++){ c = c + a; }
    return c.ToUpper();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}