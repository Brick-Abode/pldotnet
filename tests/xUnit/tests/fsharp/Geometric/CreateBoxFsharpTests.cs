using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateBoxFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateBoxFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateBoxFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("high", "POINT"), new FunctionArgument("low", "POINT") }, ReturnType = "BOX", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-box", "createBoxFSharp", "POINT '(2.052787, 3.005716)', POINT '(0.025988, 1.021653)'", "= BOX '(2.052787, 3.005716), (0.025988, 1.021653)'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBoxFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateBoxFsharpTestsFSharp : BaseCreateBoxFsharpTests
{
    protected override string FunctionBody => @"
NpgsqlBox(high, low)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}