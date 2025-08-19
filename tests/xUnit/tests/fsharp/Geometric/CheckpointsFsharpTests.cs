using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCheckpointsFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCheckpointsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CheckpointsFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-point", "checkPointsFSharp1", "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345789)", "is true" }, new object[] { "f#-point", "checkPointsFSharp2", "POINT(2.555701574,8.7552345789),POINT(2.555701574,8.7552345785)", "is false" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCheckpointsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CheckpointsFsharpTestsFSharp : BaseCheckpointsFsharpTests
{
    protected override string FunctionBody => @"
pointa.X = pointb.X && pointa.Y = pointb.Y
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}