using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseBoxFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseBoxFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseBoxFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "BOX") }, ReturnType = "BOX", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-box", "increaseBoxFSharp1", "BOX(POINT(100,100),POINT(1,1))", "= BOX(POINT(101,101),POINT(2,2))" }, new object[] { "f#-box-null", "increaseBoxFSharp1", "NULL::BOX", "= BOX(POINT(101,101),POINT(1,1))" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseBoxFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreaseBoxFsharpTestsFSharp : BaseIncreaseBoxFsharpTests
{
    protected override string FunctionBody => @"
    if orig_value.HasValue then
        NpgsqlBox(
            NpgsqlPoint((orig_value.Value).UpperRight.X + 1.0, (orig_value.Value).UpperRight.Y + 1.0),
            NpgsqlPoint((orig_value.Value).LowerLeft.X + 1.0, (orig_value.Value).LowerLeft.Y + 1.0))
    else
        NpgsqlBox(NpgsqlPoint(0, 0), NpgsqlPoint(100, 100))
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}