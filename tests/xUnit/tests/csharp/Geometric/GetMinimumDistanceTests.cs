using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseGetMinimumDistanceTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseGetMinimumDistanceTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "GetMinimumDistance", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_line", "LINE"), new FunctionArgument("orig_point", "POINT") }, ReturnType = "double precision", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-line", "getMinimumDistance", "LINE '{4.0, 6.0, 2.0}', POINT(3.0,-6.0)", "= double precision '3.05085107923876'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetMinimumDistance(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class GetMinimumDistanceTestsCSharp : BaseGetMinimumDistanceTests
{
    protected override string FunctionBody => @"
double a = orig_line.A;
double b = orig_line.B;
double c = orig_line.C;
return Math.Abs((a*orig_point.X + b*orig_point.Y + c)/Math.Sqrt(a*a+b*b));
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}