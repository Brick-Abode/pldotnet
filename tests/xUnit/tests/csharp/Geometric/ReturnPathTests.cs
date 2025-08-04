using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnPathTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnPathTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnPath", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_path", "PATH") }, ReturnType = "PATH", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-path", "returnPath - open", "PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'", " <= PATH '[(1.5,2.75),(3.0,4.75),(5.0,5.0)]'" }, new object[] { "c#-path", "returnPath - close", "PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'", "<= PATH '((1.5,2.75),(3.0,4.75),(5.0,5.0))'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnPath(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class ReturnPathTestsCSharp : BaseReturnPathTests
{
    protected override string FunctionBody => @"
return orig_path;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}