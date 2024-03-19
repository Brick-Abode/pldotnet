
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CheckpointsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
pointa.X = pointb.X && pointa.Y = pointb.Y
    ";

    public CheckpointsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CheckpointsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "f#-point",
                "checkPointsFSharp1",
                "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345789)",
                "is true"
            },
            new object[] {
                "f#-point",
                "checkPointsFSharp2",
                "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345785)",
                "is false"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCheckpointsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
