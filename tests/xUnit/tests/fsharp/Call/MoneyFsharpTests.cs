using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMoneyFSharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMoneyFSharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "testMoneyFSharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "MONEY") }, ReturnType = "MONEY", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-money", "testMoneyFSharp1", "'32500.0'::MONEY", " = '32500.0'::MONEY" }, new object[] { "f#-money", "testMoneyFSharp1", "NULL::MONEY", " = 0::MONEY" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestNone(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Call")]
public class MoneyFSharpTestsFSharp : BaseMoneyFSharpTests
{
    protected override string FunctionBody => @"
let a = if a.HasValue then a.Value else 0
a
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}