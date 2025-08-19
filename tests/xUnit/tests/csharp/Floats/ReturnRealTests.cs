using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnRealTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnRealTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnReal", Arguments = new List<FunctionArgument> { }, ReturnType = "real", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float4", "returnReal", "", "= real '1.50055'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnReal(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnRealTestsCSharp : BaseReturnRealTests
{
    protected override string FunctionBody => @"
return 1.50055f;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}