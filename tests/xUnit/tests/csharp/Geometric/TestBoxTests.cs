using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseTestBoxTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseTestBoxTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "TestBox", Arguments = new List<FunctionArgument> { new FunctionArgument("my_box", "BOX") }, ReturnType = "BOX", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-box", "testBox", "BOX '(0.025988, 1.021653), (2.052787, 3.005716)'", "= BOX '(0.025988, 1.021653), (2.052787, 3.005716)'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTestBox(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class TestBoxTestsCSharp : BaseTestBoxTests
{
    protected override string FunctionBody => @"
return my_box;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}