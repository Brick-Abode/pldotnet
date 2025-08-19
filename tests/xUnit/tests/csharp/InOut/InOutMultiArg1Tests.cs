using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseInOutMultiArg1Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseInOutMultiArg1Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "InOutMultiArg1", Arguments = new List<FunctionArgument> { new FunctionArgument("IN argument_0", "INT"), new FunctionArgument("INOUT argument_1", "INT"), new FunctionArgument("IN argument_2", "INT"), new FunctionArgument("OUT argument_3", "INT"), new FunctionArgument("OUT argument_4", "INT"), new FunctionArgument("INOUT argument_5", "INT"), new FunctionArgument("IN argument_6", "INT"), new FunctionArgument("OUT argument_7", "INT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inout-multiarg-1", "inout_multiarg_1", "0, 1, 2, NULL, 6", "= ROW(2, 4, 5, 6, NULL::INT)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutMultiArg1(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutMultiArg1TestsCSharp : BaseInOutMultiArg1Tests
{
    protected override string FunctionBody => @"
if(argument_0 != 0){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    if(argument_1 != 1){ throw new SystemException($""Failed assertion: argument_1 = {argument_1}"");}
    argument_1 = 2;
    if(argument_2 != 2){ throw new SystemException($""Failed assertion: argument_2 = {argument_2}"");}
    argument_3 = 4;
    argument_4 = 5;
    if(argument_5 != null){ throw new SystemException($""Failed assertion: argument_5 = {argument_5}"");}
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($""Failed assertion: argument_6 = {argument_6}"");}
    argument_7 = null;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}