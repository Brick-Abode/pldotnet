using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnMinMoneyTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnMinMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnMinMoney", Arguments = new List<FunctionArgument> { }, ReturnType = "MONEY", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-money", "returnMinMoney", "", "= '-92233720368547758.08'::MONEY" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnMinMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class ReturnMinMoneyTestsCSharp : BaseReturnMinMoneyTests
{
    protected override string FunctionBody => @"
decimal value = -92233720368547758.08M;
    return value;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}