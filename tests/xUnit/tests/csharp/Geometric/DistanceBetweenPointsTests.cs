using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseDistanceBetweenPointsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseDistanceBetweenPointsTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "DistanceBetweenPoints", Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") }, ReturnType = "double precision", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-point", "distanceBetweenPoints", "POINT(1.5,2.75), POINT(3.0,4.75)", "= double precision '2.5'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDistanceBetweenPoints(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class DistanceBetweenPointsTestsCSharp : BaseDistanceBetweenPointsTests
{
    protected override string FunctionBody => @"
double dif_x = (pointa.X - pointb.X);
double dif_y = (pointa.Y - pointb.Y);
double distance = Math.Sqrt(dif_x*dif_x+dif_y*dif_y);
return distance;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}