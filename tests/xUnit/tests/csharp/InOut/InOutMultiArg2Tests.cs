
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutMultiArg2Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(argument_0 != null){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = 1;
    argument_1 = 2;
    if(argument_2 != 2){ throw new SystemException($""Failed assertion: argument_2 = {argument_2}"");}
    argument_2 = 3;
    if(argument_3 != 3){ throw new SystemException($""Failed assertion: argument_3 = {argument_3}"");}
    argument_3 = null;
    if(argument_4 != 4){ throw new SystemException($""Failed assertion: argument_4 = {argument_4}"");}
    argument_5 = 6;
    if(argument_6 != 6){ throw new SystemException($""Failed assertion: argument_6 = {argument_6}"");}
    argument_7 = 8;
    ";

    public InOutMultiArg2Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutMultiArg2",
            Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT"), new FunctionArgument("OUT argument_1", "INT"), new FunctionArgument("INOUT argument_2", "INT"), new FunctionArgument("INOUT argument_3", "INT"), new FunctionArgument("IN argument_4", "INT"), new FunctionArgument("OUT argument_5", "INT"), new FunctionArgument("IN argument_6", "INT"), new FunctionArgument("OUT argument_7", "INT") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inout-multiarg-2", "inout_multiarg_2", "NULL, 2, 3, 4, 6", "= ROW(1, 2, 3, NULL::INT, 6, 8)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutMultiArg2(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
