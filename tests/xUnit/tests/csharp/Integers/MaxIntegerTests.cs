using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxIntegerTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxIntegerTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxInteger", Arguments = new List<FunctionArgument> { }, ReturnType = "integer", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4", "maxInteger", "", "= integer '2147483647'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxInteger(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MaxIntegerTestsCSharp : BaseMaxIntegerTests
{
    protected override string FunctionBody => @"
return 2147483647;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}