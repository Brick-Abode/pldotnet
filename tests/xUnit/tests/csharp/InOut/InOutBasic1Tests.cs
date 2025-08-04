using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutBasic1Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutBasic1Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutBasic1", Arguments = new List<FunctionArgument> { new FunctionArgument("OUT argument_0", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-basic-1", "inout_basic_1", "", "= 1" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutBasic1(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutBasic1TestsCSharp : BaseInOutBasic1Tests
{
    protected override string FunctionBody => @"
argument_0 = 1;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}