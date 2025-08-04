using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutNull4Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutNull4Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutNull4", Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT"), new FunctionArgument("in argument_1", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-null-4", "inout_null_4", "11, 3", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull4(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull4TestsCSharp : BaseInOutNull4Tests
{
    protected override string FunctionBody => @"
if(argument_0 is null ){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    if(argument_1 != 3){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = null;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}