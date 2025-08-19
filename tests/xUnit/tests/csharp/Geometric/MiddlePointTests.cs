using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMiddlePointTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMiddlePointTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MiddlePoint", Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") }, ReturnType = "point", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-point", "middlePoint1", "POINT(10.0,20.0),POINT(20.0,40.0)", "~= POINT(15.0,30.0)" }, new object[] { "c#-point-null", "middlePoint2", "NULL::POINT,POINT(20.0,40.0)", "~= POINT(10.0,20.0)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMiddlePoint(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class MiddlePointTestsCSharp : BaseMiddlePointTests
{
    protected override string FunctionBody => @"
if (pointa == null)
    pointa = new NpgsqlPoint(0, 0);

if (pointb == null)
    pointb = new NpgsqlPoint(0, 0);

double x = (((NpgsqlPoint)pointa).X + ((NpgsqlPoint)pointb).X)*0.5;
double y = (((NpgsqlPoint)pointa).Y + ((NpgsqlPoint)pointb).Y)*0.5;
var new_point = new NpgsqlPoint(x,y);
return new_point;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}