
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CheckpointsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(pointa.X == pointb.X && pointa.Y == pointb.Y)
{
    return true;
}
return false;
    ";

    public CheckpointsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Checkpoints",
            Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-point", "checkPoints1", "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345789)", " is true" },
            new object[] { "c#-point", "checkPoints2", "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345785)", "is false" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCheckpoints(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
