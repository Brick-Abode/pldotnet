
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class SumSmallInTArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_small_integers = Array.CreateInstance(typeof(object), small_integers.Length);
ArrayManipulation.FlatArray(small_integers, ref flatten_small_integers);
short small_integers_sum = (short)0;
for(int i = 0; i < flatten_small_integers.Length; i++)
{
    if (flatten_small_integers.GetValue(i) == null)
        continue;
    small_integers_sum = (short)(small_integers_sum + (short)flatten_small_integers.GetValue(i));
}
return small_integers_sum;
    ";

    public SumSmallInTArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumSmallInTArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("small_integers", "smallint[]") },
            ReturnType = "smallint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int2-null-1array", "sumSmallIntArray1", "ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]", "= '16824'" },
        new object[] { "c#-int2-null-2array-arraynull", "sumSmallIntArray2", "ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]", "= '12999'" },
        new object[] { "c#-int2-null-3array-arraynull", "sumSmallIntArray3", "ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 13823::smallint], [9521::smallint, 934::smallint]]]", "= '24464'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumSmallInTArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
