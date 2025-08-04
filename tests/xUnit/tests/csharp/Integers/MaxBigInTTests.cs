using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxBigInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxBigInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxBigInT", Arguments = new List<FunctionArgument> { }, ReturnType = "bigint", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8", "maxBigInt", "", "= bigint '9223372036854775807'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxBigInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MaxBigInTTestsCSharp : BaseMaxBigInTTests
{
    protected override string FunctionBody => @"
return 9223372036854775807;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}