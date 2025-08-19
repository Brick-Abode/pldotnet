using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSum2BigInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSum2BigInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "Sum2BigInT", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "bigint"), new FunctionArgument("b", "bigint") }, ReturnType = "bigint", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8", "sum2BigInt1", "9223372036854775707, 100", "= bigint '9223372036854775807'" }, new object[] { "c#-int8-null", "sum2BigInt2", "9223372036854775707::BIGINT, NULL::BIGINT", "= bigint '9223372036854775707'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2BigInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class Sum2BigInTTestsCSharp : BaseSum2BigInTTests
{
    protected override string FunctionBody => @"
if (a == null)
    a = 0;

if (b == null)
    b = 0;

return a+b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}