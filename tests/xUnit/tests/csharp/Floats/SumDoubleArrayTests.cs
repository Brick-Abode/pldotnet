
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class SumDoubleArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_doubles = Array.CreateInstance(typeof(object), doubles.Length);
ArrayManipulation.FlatArray(doubles, ref flatten_doubles);
double double_sum = 0;
for(int i = 0; i < flatten_doubles.Length; i++)
{
    if (flatten_doubles.GetValue(i) == null)
        continue;
    double_sum = double_sum + (double)flatten_doubles.GetValue(i);
}
return double_sum;
    ";

    public SumDoubleArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumDoubleArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("doubles double", "precision[]") },
            ReturnType = "double precision",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float8-null-1array", "sumDoubleArray1", "ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]", "= '32.9335786424099'" },
        new object[] { "c#-float8-null-2array", "sumDoubleArray2", "ARRAY[[21.0000000000109::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]", "= '32.9335786424099'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumDoubleArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
