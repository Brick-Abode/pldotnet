
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class InOutObject10FsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a + "" "" + b;
    ";

    public InOutObject10FsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "InOutObject10Fs",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "text"), new FunctionArgument("INOUT b", "text") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-inout-object-10", "inout_object_10_fs", "'red', 'blue'", "= 'red blue'" },
        new object[] { "f#-inout-object-11", "inout_object_10_fs", "'red', NULL", "= 'red '" },
        new object[] { "f#-inout-object-12", "inout_object_10_fs", "NULL, 'blue'", "= ' blue'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestInOutObject10Fs(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
