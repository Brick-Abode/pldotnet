
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class CountBoolTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_booleans = Array.CreateInstance(typeof(object), booleans.Length);
ArrayManipulation.FlatArray(booleans, ref flatten_booleans);
int count = 0;
for(int i = 0; i < flatten_booleans.Length; i++)
{
    if (flatten_booleans.GetValue(i) == null)
        continue;
    if((bool)flatten_booleans.GetValue(i) == desired)
        count++;
}
return count;
    ";

    public CountBoolTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CountBool",
            Arguments = new List<FunctionArgument> { new FunctionArgument("booleans", "boolean[]"), new FunctionArgument("desired", "boolean") },
            ReturnType = "Integer",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bool-null-1array", "countBool1", "ARRAY[true, true, false, true, null::boolean], true", "= integer '3'" },
            new object[] { "c#-bool-null-1array", "countBool2", "ARRAY[true, true, false, true, null::boolean], false", "= integer '1'" },
            new object[] { "c#-bool-null-2array", "countBool3", "ARRAY[[true, null::boolean, true], [true, false, null::boolean]], true", "= integer '3'" },
            new object[] { "c#-bool-null-2array", "countBool4", "ARRAY[[true, null::boolean, true], [true, false, null::boolean]], false", "= integer '1'" },
            new object[] { "c#-bool-null-3array", "countBool5", "ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], true", "= integer '5'" },
            new object[] { "c#-bool-null-3array", "countBool6", "ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], false", "= integer '3'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCountBool(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
