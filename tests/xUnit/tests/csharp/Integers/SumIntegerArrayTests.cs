using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSumIntegerArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSumIntegerArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SumIntegerArray", Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "integer[]") }, ReturnType = "integer", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4-null-1array", "sumIntegerArray1", "ARRAY[2047483647::integer, null::integer, 304325::integer, 4356::integer]", "= '2047792328'" }, new object[] { "c#-int4-null-2array-arraynull", "sumIntegerArray2", "ARRAY[[null::integer, null::integer], [2047483647::integer, 304325::integer]]", "= '2047787972'" }, new object[] { "c#-int4-null-3array-arraynull", "sumIntegerArray3", "ARRAY[[[null::integer, null::integer], [null::integer, null::integer]], [[2047483647::integer, 304325::integer], [706524::integer, 4356::integer]]]", "= '2048498852'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumIntegerArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class SumIntegerArrayTestsCSharp : BaseSumIntegerArrayTests
{
    protected override string FunctionBody => @"
Array flatten_integers = Array.CreateInstance(typeof(object), integers.Length);
ArrayManipulation.FlatArray(integers, ref flatten_integers);
int integers_sum = 0;
for(int i = 0; i < flatten_integers.Length; i++)
{
    if (flatten_integers.GetValue(i) == null)
        continue;
    integers_sum = integers_sum + (int)flatten_integers.GetValue(i);
}
return integers_sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}