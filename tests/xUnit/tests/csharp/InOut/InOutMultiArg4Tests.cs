
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutMultiArg4Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(argument_0 != 0){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
        if(argument_1 != 1){ throw new SystemException($""Failed assertion: argument_1 = {argument_1}"");}
        argument_1 = 2;
        if(argument_2 != 2){ throw new SystemException($""Failed assertion: argument_2 = {argument_2}"");}
        argument_3 = 4;
        argument_4 = 5;
        if(argument_5 != 5){ throw new SystemException($""Failed assertion: argument_5 = {argument_5}"");}
        argument_5 = null;
        if(argument_6 != 6){ throw new SystemException($""Failed assertion: argument_6 = {argument_6}"");}
        argument_6 = 7;
        if(argument_7 != 7){ throw new SystemException($""Failed assertion: argument_7 = {argument_7}"");}
    ";

    public InOutMultiArg4Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutMultiArg4",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN argument_0", "INT"), new FunctionArgument("INOUT argument_1", "INT"), new FunctionArgument("IN argument_2", "INT"), new FunctionArgument("OUT argument_3", "INT"), new FunctionArgument("OUT argument_4", "INT"), new FunctionArgument("INOUT argument_5", "INT"), new FunctionArgument("INOUT argument_6", "INT"), new FunctionArgument("IN argument_7", "INT") },
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
            new object[] { "c#-inout-multiarg-4", "inout_multiarg_4", "0, 1, 2, 5, 6, 7", "= ROW(2, 4, 5, NULL::INT, 7)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutMultiArg4(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
