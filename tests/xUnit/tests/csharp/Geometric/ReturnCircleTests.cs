using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnCircleTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnCircleTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnCircle", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_circle", "CIRCLE") }, ReturnType = "CIRCLE", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-circle", "returnCircle", "CIRCLE '2.5, 3.5, 12.78'", "~= CIRCLE '<(2.5, 3.5), 12.78>'" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnCircle(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ReturnCircleTestsCSharp : BaseReturnCircleTests
{
    protected override string FunctionBody => @"
return orig_circle;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}