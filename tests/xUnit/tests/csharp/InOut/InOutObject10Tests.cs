
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutObject10Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null) a = """";
    if (b == null) b = """";
    b = a + "" "" + b;
    ";

    public InOutObject10Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutObject10",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "text"), new FunctionArgument("INOUT b", "text") },
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
            new object[] { "c#-inout-object-10", "inout_object_10", "'red', 'blue'", "= 'red blue'" },
        new object[] { "c#-inout-object-11", "inout_object_10", "'red', NULL", "= 'red '" },
        new object[] { "c#-inout-object-12", "inout_object_10", "NULL, 'blue'", "= ' blue'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutObject10(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
