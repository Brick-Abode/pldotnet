
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class Sum2IntegerTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
    ";

    public Sum2IntegerTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2Integer",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer") },
            ReturnType = "integer",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4", "sum2Integer1", "32770, 100", "= INTEGER '32870'" },
        new object[] { "c#-int4-null", "sum2Integer2", "NULL::INTEGER, 100::INTEGER", "= INTEGER '100'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2Integer(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
