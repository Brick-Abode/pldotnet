using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnBoolTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnBoolTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnBool", Arguments = new List<FunctionArgument> { }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bool", "returnBool", "", " is false" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBool(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class ReturnBoolTestsCSharp : BaseReturnBoolTests
{
    protected override string FunctionBody => @"
return false;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}