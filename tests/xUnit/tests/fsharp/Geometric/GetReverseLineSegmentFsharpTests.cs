using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseGetReverseLineSegmentFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseGetReverseLineSegmentFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "GetReverseLineSegmentFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("my_line", "LSEG") }, ReturnType = "LSEG", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-lseg", "getReverseLineSegmentFSharp1", "LSEG(POINT(0.0,1.0),POINT(5.0,3.0))", "= LSEG '[(5.0,3.0),(0.0,1.0)]'" }, new object[] { "f#-lseg-null", "getReverseLineSegmentFSharp2", "NULL::LSEG", "= LSEG '[(100.0,100.0),(0.0,0.0)]'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetReverseLineSegmentFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class GetReverseLineSegmentFsharpTestsFSharp : BaseGetReverseLineSegmentFsharpTests
{
    protected override string FunctionBody => @"
let my_line = if my_line.HasValue then my_line.Value else NpgsqlLSeg(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100))

let firstPoint = my_line.Start
let secondPoint = my_line.End
let newLine = new NpgsqlLSeg(secondPoint, firstPoint)
newLine
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}