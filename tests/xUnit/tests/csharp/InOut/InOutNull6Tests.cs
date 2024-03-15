
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutNull6Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(argument_0 == 1){ argument_1 = null; }
    else if(argument_0 == 2){ argument_1 = 2; }
    else { throw new SystemException($""Failed assertion: argument_0 = {argument_0}"");}
    ";

    public InOutNull6Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutNull6",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN argument_0", "INT"), new FunctionArgument("OUT argument_1", "INT") },
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
            new object[] { "c#-inout-null-6", "inout_null_6", "1", "IS NULL" },
        new object[] { "c#-inout-null-7", "inout_null_6", "2", "= 2" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutNull6(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
