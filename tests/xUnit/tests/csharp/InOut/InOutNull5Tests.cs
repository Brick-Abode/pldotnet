
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull5Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(argument_0 != null ){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    if(argument_1 != 3){ throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    argument_0 = 3;
    ";

    public InOutNull5Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutNull5",
            Arguments = new List<FunctionArgument> { new FunctionArgument("INOUT argument_0", "INT"), new FunctionArgument("IN argument_1", "INT") },
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
            new object[] { "c#-inout-null-5", "inout_null_5", "NULL, 3", "= 3" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull5(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
