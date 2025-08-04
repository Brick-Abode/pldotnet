using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIdentityStrFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIdentityStrFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IdentityStrFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "text") }, ReturnType = "text", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-text", "identityStrFSharp", "'dog'", "= 'dog'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIdentityStrFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class IdentityStrFsharpTestsFSharp : BaseIdentityStrFsharpTests
{
    protected override string FunctionBody => @"
a
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}