
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class DistanceBetweenPointsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
double dif_x = (pointa.X - pointb.X);
double dif_y = (pointa.Y - pointb.Y);
double distance = Math.Sqrt(dif_x*dif_x+dif_y*dif_y);
return distance;
    ";

    public DistanceBetweenPointsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "DistanceBetweenPoints",
            Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") },
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
            new object[] { "c#-point", "distanceBetweenPoints", "POINT(1.5,2.75), POINT(3.0,4.75)", "= double precision '2.5'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDistanceBetweenPoints(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
