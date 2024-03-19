
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class SumRealTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
    ";

    public SumRealTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumReal",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "real"), new FunctionArgument("b", "real") },
            ReturnType = "real",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float4", "sumReal1", "1.50055, 1.50054", "= real '3.00109'" },
        new object[] { "c#-float4-null", "sumReal2", "NULL, 1.50054", "= real '1.50054'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumReal(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
