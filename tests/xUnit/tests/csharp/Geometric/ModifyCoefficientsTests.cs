
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ModifyCoefficientsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (original_line == null)
    original_line = new NpgsqlLine(2.4, 8.2, -32.43);

double a = ((NpgsqlLine)original_line).A * -1.0;
double b = ((NpgsqlLine)original_line).B * -1.0;
double c = ((NpgsqlLine)original_line).C * -1.0;
NpgsqlLine my_line = new NpgsqlLine(a,b,c);
return my_line;
    ";

    public ModifyCoefficientsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyCoefficients",
            Arguments = new List<FunctionArgument> { new FunctionArgument("original_line", "LINE") },
            ReturnType = "LINE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-line", "modifyCoefficients1", "LINE '{-1.5,2.75,-3.25}'", "= LINE '{1.50,-2.75,3.25}'" },
            new object[] { "c#-line-null", "modifyCoefficients2", "NULL::LINE", "= LINE '{2.4, 8.2, -32.43}'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyCoefficients(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
