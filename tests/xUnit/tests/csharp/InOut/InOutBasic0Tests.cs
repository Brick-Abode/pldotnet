using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutBasic0Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutBasic0Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutBasic0", Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-basic-0", "inout_basic_0", "0", "= 1" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutBasic0(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutBasic0TestsCSharp : BaseInOutBasic0Tests
{
    protected override string FunctionBody => @"
if(argument_0 != 0){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = 1;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}