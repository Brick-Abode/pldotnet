
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class AddSmileTovarCharsTests : PlDotNetTest
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

    public AddSmileTovarCharsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddSmileTovarChars",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "VARCHAR[]") },
            ReturnType = "VARCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varchar-null-1array", "AddSmileToVarchars1", "ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, null::VARCHAR, 'bye'::VARCHAR]", "= ARRAY['hello :)'::VARCHAR, 'hi :)'::VARCHAR, null::VARCHAR, 'bye :)'::VARCHAR]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddSmileTovarChars(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
