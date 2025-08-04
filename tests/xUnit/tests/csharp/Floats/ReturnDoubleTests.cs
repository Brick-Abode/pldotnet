using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnDoubleTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnDoubleTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnDouble", Arguments = new List<FunctionArgument> { }, ReturnType = "double precision", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8", "returnDouble", "", "= double precision '11.0050000000005'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnDouble(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnDoubleTestsCSharp : BaseReturnDoubleTests
{
    protected override string FunctionBody => @"
return 11.0050000000005;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}