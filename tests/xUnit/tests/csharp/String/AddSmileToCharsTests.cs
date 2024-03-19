
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class AddSmileToCharsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i);
    string new_value = orig_value + "" :)"";

    flatten_values.SetValue((string)new_value, i);
}
return flatten_values;
    ";

    public AddSmileToCharsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddSmileToChars",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BPCHAR[]") },
            ReturnType = "BPCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bpchar-null-1array", "AddSmileToChars1", "ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR]", "= ARRAY['hello :)'::BPCHAR, 'hi :)'::BPCHAR, null::BPCHAR, 'bye :)'::BPCHAR]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddSmileToChars(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
