
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateLineSegmentFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlLSeg(start_point, end_point)
    ";

    public CreateLineSegmentFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLineSegmentFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("start_point", "POINT"), new FunctionArgument("end_point", "POINT") },
            ReturnType = "LSEG",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-lseg", "createLineSegmentFSharp", "POINT(0.088997,1.258456),POINT(5.456102,3.04561)", "= LSEG '[(0.088997,1.258456),(5.456102,3.04561)]'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineSegmentFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
