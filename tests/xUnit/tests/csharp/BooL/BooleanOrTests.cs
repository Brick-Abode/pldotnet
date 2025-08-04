using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseBooleanOrTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseBooleanOrTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "BooleanOr", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bool", "BooleanOr1", "false, false", "is false" }, new object[] { "c#-bool-null", "BooleanOr2", "true, NULL::BOOLEAN", "is true" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanOr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class BooleanOrTestsCSharp : BaseBooleanOrTests
{
    protected override string FunctionBody => @"
if (a == null) {
    a = false;
}

if (b == null) {
    b = false;
}

return a|b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}