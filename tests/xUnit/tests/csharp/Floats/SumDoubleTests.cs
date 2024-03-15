
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class SumDoubleTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
    ";

    public SumDoubleTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumDouble",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a double", "precision"), new FunctionArgument("b double", "precision") },
            ReturnType = "double precision",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float8", "sumDouble1", "10.5000000000055, 10.5000000000054", "= double precision  '21.0000000000109'" },
        new object[] { "c#-float8-null", "sumDouble2", "NULL, NULL", "= double precision '0'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumDouble(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
