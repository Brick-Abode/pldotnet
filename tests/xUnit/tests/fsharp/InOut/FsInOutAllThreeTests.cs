
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class FsInOutAllThreeTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if a.HasValue && b.HasValue then
        Nullable(a.Value + 1), Nullable(a.Value + b.Value)
    else
        Nullable(), Nullable()
    ";

    public FsInOutAllThreeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "FsInOutAllThree",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "INT"), new FunctionArgument("INOUT b", "INT"), new FunctionArgument("OUT c", "INT") },
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
            new object[] { "f#-inout-allthree-1", "fs_inout_allthree", "11, 8", "= ROW(12,19)" },
        new object[] { "f#-inout-allthree-2", "fs_inout_allthree", "NULL::int, 8", "= ROW(NULL::int, NULL::int)" },
        new object[] { "f#-inout-allthree-3", "fs_inout_allthree", "8, NULL::int", "= ROW(NULL::int, NULL::int)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestFsInOutAllThree(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
