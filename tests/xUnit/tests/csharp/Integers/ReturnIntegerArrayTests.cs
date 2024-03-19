
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class ReturnIntegerArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return integers;
    ";

    public ReturnIntegerArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnIntegerArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "integer[]") },
            ReturnType = "integer[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4-null-1array", "returnIntegerArray1", "ARRAY[2047483647::integer, null::integer, 304325::integer, 706524::integer]", "= ARRAY[2047483647::integer, null::integer, 304325::integer, 706524::integer]" },
        new object[] { "c#-int4-null-2array-arraynull", "returnIntegerArray2", "ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]]", "= ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]]" },
        new object[] { "c#-int4-null-3array-arraynull", "returnIntegerArray3", "ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 934::integer]]]", "= ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 934::integer]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnIntegerArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
