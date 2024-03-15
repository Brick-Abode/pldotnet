
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Json")]
public class ReplaceJSonsKeyTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i);
    string new_value = orig_value.Replace(""name"", ""first_name"");

    flatten_values.SetValue((string)new_value, i);
}
return flatten_values;
    ";

    public ReplaceJSonsKeyTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReplaceJSonsKey",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "JSON[]") },
            ReturnType = "JSON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[]
            {
                "c#-json-null-1array",
                "ReplaceJsonsKey1",
                "ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]",
                "= ARRAY['{\"age\": 20, \"first_name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"first_name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"first_name\": \"Todd\"}'::JSON]::TEXT"
            }
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReplaceJSonsKey(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
