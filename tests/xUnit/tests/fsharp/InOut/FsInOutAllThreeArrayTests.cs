
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "InOut")]
public class FsInOutAllThreeArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let c = b + 1
    let arr = Array.CreateInstance(typeof<int16>, 3, 3)
    arr.SetValue((int16)a, 0, 0)
    arr.SetValue((int16)a, 1, 1)
    arr.SetValue((int16)a, 2, 2)
    Nullable(c), arr
    ";

    public FsInOutAllThreeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "FsInOutAllThreeArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("IN a", "INT"), new FunctionArgument("INOUT b", "INT"), new FunctionArgument("OUT c", "int2[]") },
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-inout-allthreearray", "fs_inout_allthreearray", "3, 8", "= ROW(9, ARRAY[[3::int2,0::int2,0::int2], [0::int2, 3::int2, 0::int2], [0::int2, 0::int2, 3::int2]])" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestFsInOutAllThreeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
