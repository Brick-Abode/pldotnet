using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutNull3Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutNull3Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutNull3", Arguments = new List<FunctionArgument> { new FunctionArgument("OUT argument_0", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-null-3", "inout_null_3", "", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull3(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull3TestsCSharp : BaseInOutNull3Tests
{
    protected override string FunctionBody => @"
argument_0 = null;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}