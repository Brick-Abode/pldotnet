
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class MultiplyVarCharTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string c = """";
    for(int i=0;i<b;i++){ c = c + a; }
    return c.ToUpper();
    ";

    public MultiplyVarCharTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MultiplyVarChar",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "int") },
            ReturnType = "VARCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varchar", "multiplyVarChar", "'hello '::VARCHAR, 5", "= 'HELLO HELLO HELLO HELLO HELLO '::VARCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyVarChar(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
