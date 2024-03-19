
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutMultiArg3Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
argument_0 = null;
    if(argument_1 != 1){ throw new SystemException($""Failed assertion: argument_1 = {argument_1}"");}
    if(argument_2 != 2){ throw new SystemException($""Failed assertion: argument_2 = {argument_2}"");}
    argument_3 = 4;
    if(argument_4 != 4){ throw new SystemException($""Failed assertion: argument_4 = {argument_4}"");}
    argument_4 = 5;
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($""Failed assertion: argument_6 = {argument_6}"");}
    if(argument_7 != 7){ throw new SystemException($""Failed assertion: argument_7 = {argument_7}"");}
    argument_7 = 8;
    ";

    public InOutMultiArg3Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutMultiArg3",
            Arguments = new List<FunctionArgument> { new FunctionArgument("OUT argument_0", "INT"), new FunctionArgument("IN argument_1", "INT"), new FunctionArgument("IN argument_2", "INT"), new FunctionArgument("OUT argument_3", "INT"), new FunctionArgument("INOUT argument_4", "INT"), new FunctionArgument("OUT argument_5", "INT"), new FunctionArgument("IN argument_6", "INT"), new FunctionArgument("INOUT argument_7", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inout-multiarg-3", "inout_multiarg_3", "1, 2, 4, 6, 7", "= ROW(NULL::INT, 4, 5, 6, 8)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutMultiArg3(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
