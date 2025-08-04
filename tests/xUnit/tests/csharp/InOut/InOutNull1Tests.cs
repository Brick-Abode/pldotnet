using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutNull1Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutNull1Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutNull1", Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-null-1", "inout_null_1", "11", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull1(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull1TestsCSharp : BaseInOutNull1Tests
{
    protected override string FunctionBody => @"
if(argument_0 is null ){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = null;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}