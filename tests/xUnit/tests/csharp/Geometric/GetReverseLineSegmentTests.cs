
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class GetReverseLineSegmentTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (my_line == null)
    my_line = new NpgsqlLSeg(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100));

NpgsqlPoint firstPoint = ((NpgsqlLSeg)my_line).Start;
NpgsqlPoint secondPoint = ((NpgsqlLSeg)my_line).End;
NpgsqlLSeg newLine = new NpgsqlLSeg(secondPoint, firstPoint);
return newLine;
    ";

    public GetReverseLineSegmentTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "GetReverseLineSegment",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_line", "LSEG") },
            ReturnType = "LSEG",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-lseg", "getReverseLineSegment1", "LSEG(POINT(0.0,1.0),POINT(5.0,3.0))", "= LSEG '[(5.0,3.0),(0.0,1.0)]'" },
            new object[] { "c#-lseg-null", "getReverseLineSegment2", "NULL::LSEG", "= LSEG '[(100.0,100.0),(0.0,0.0)]'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetReverseLineSegment(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
