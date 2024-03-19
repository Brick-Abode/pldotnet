
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseLinesTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    NpgsqlLine orig_value = (NpgsqlLine)flatten_values.GetValue(i);
    NpgsqlLine new_value = new NpgsqlLine(orig_value.A + 1, orig_value.B + 1, orig_value.C + 1);

    flatten_values.SetValue((NpgsqlLine)new_value, i);
}
return flatten_values;
    ";

    public IncreaseLinesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseLines",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LINE[]") },
            ReturnType = "LINE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-line-1array", "IncreaseLines1", "ARRAY[LINE '{-4.5,5.75,-7.25}', LINE '{-46.5,32.75,-54.5}', null::LINE, LINE '{-1.5,2.75,-3.25}']", "= CAST(ARRAY[LINE '{-3.5,6.75,-6.25}', LINE '{-45.5,33.75,-53.5}', null::LINE, LINE '{-0.5,3.75,-2.25}'] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseLines(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
