
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ReturnWidthTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlBox new_box = new NpgsqlBox(high, low);
return (double)Math.Abs(new_box.Width);
    ";

    public ReturnWidthTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnWidth",
            Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") },
            ReturnType = "double precision",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-box", "returnWidth", "POINT '(0.025988, 1.021653)', POINT '(2.052787, 3.005716)'", "= double precision '2.026799'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnWidth(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
