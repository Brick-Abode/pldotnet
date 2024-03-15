
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class ReturnBooleanArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return booleans;
    ";

    public ReturnBooleanArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnBooleanArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("booleans", "boolean[]") },
            ReturnType = "boolean[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bool-null-1array", "returnBooleanArray1", "ARRAY[true, null::boolean, false, false]", "= ARRAY[true, null::boolean, false, false]" },
            new object[] { "c#-bool-null-2array-arraynull", "returnBooleanArray2", "ARRAY[[true, false], [null::boolean, null::boolean]]", "= ARRAY[[true, false], [null::boolean, null::boolean]]" },
            new object[] { "c#-bool-null-3array-arraynull", "returnBooleanArray3", "ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]]", "= ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBooleanArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
