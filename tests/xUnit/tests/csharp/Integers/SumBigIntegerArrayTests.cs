
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class SumBigIntegerArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_big_integers = Array.CreateInstance(typeof(object), big_integers.Length);
ArrayManipulation.FlatArray(big_integers, ref flatten_big_integers);
long big_integers_sum = 0;
for(int i = 0; i < flatten_big_integers.Length; i++)
{
    if (flatten_big_integers.GetValue(i) == null)
        continue;
    big_integers_sum = (long)(big_integers_sum + (long)flatten_big_integers.GetValue(i));
}
return big_integers_sum;
    ";

    public SumBigIntegerArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumBigIntegerArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("big_integers", "bigint[]") },
            ReturnType = "bigint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8-null-1array", "sumBigIntegerArray1", "ARRAY[223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 4356::bigint]", "= '246744073709555770'" },
        new object[] { "c#-int8-null-2array-arraynull", "sumBigIntegerArray2", "ARRAY[[null::bigint, null::bigint], [92332036854775707::bigint, 23372036854775707::bigint]]", "= '115704073709551414'" },
        new object[] { "c#-int8-null-3array-arraynull", "sumBigIntegerArray3", "ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[92232036854775707::bigint, 2337203684775707::bigint], [706524::bigint, 756452434247987::bigint]]]", "= '95325692974505925'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumBigIntegerArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
