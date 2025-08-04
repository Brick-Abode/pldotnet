using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIdentityStrTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIdentityStrTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IdentityStr", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text", "identityStr", "'dog'", "= 'dog'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIdentityStr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class IdentityStrTestsCSharp : BaseIdentityStrTests
{
    protected override string FunctionBody => @"
System.Console.WriteLine(""Got string: {0}"", a);
    return a;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}