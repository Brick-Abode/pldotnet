using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutNull2Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutNull2Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutNull2", Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-null-2", "inout_null_2", "NULL", "= 3" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull2(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull2TestsCSharp : BaseInOutNull2Tests
{
    protected override string FunctionBody => @"
if(argument_0 != null ){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = 3;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}