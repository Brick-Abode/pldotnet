
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class GetMinimumDistanceTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
double a = orig_line.A;
double b = orig_line.B;
double c = orig_line.C;
return Math.Abs((a*orig_point.X + b*orig_point.Y + c)/Math.Sqrt(a*a+b*b));
    ";

    public GetMinimumDistanceTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "GetMinimumDistance",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_line", "LINE"), new FunctionArgument("orig_point", "POINT") },
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
            new object[] { "c#-line", "getMinimumDistance", "LINE '{4.0, 6.0, 2.0}', POINT(3.0,-6.0)", "= double precision '3.05085107923876'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetMinimumDistance(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
