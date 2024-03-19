
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "InOut")]
public class InOutObject20Tests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null) a = """";
    if (b == null) b = """";
    c = a + "" "" + b;
    ";

    public InOutObject20Tests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutObject20",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "text"), new FunctionArgument("b", "text"), new FunctionArgument("OUT c", "text") },
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
            new object[] { "c#-inout-object-20", "inout_object_20", "'red', 'blue'", "= 'red blue'" },
        new object[] { "c#-inout-object-21", "inout_object_20", "'red', NULL", "= 'red '" },
        new object[] { "c#-inout-object-22", "inout_object_20", "NULL, 'blue'", "= ' blue'" },
        new object[] { "c#-inout-object-23", "inout_object_20", "'🐂', '🥰'", "= '🐂 🥰'::TEXT" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutObject20(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
