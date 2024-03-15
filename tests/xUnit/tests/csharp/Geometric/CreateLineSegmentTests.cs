
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateLineSegmentTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlLSeg newLine = new NpgsqlLSeg(start_point, end_point);
return newLine;
    ";

    public CreateLineSegmentTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLineSegment",
            Arguments = new List<FunctionArgument> { new FunctionArgument("start_point", "POINT"), new FunctionArgument("end_point", "POINT") },
            ReturnType = "LSEG",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-lseg", "createLineSegment", "POINT(0.088997,1.258456),POINT(5.456102,3.04561)", "= LSEG '[(0.088997,1.258456),(5.456102,3.04561)]'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineSegment(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
