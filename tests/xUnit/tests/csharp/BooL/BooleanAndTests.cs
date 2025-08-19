using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseBooleanAndTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseBooleanAndTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "BooleanAnd", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") }, ReturnType = "boolean", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bool", "BooleanAnd1", "true, true", "is true" }, new object[] { "c#-bool-null", "BooleanAnd2", "NULL::BOOLEAN, true", "is false" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanAnd(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class BooleanAndTestsCSharp : BaseBooleanAndTests
{
    protected override string FunctionBody => @"
if (a == null) {
    a = false;
}

if (b == null) {
    b = false;
}

return a&b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}