
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Byea")]
public class ConvertByTeaArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
UTF8Encoding utf8_e = new UTF8Encoding();
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    byte[] orig_value = (byte[])flatten_values.GetValue(i);
    string s1 = utf8_e.GetString(orig_value, 0, orig_value.Length);
    byte[] new_value = utf8_e.GetBytes(s1);

    flatten_values.SetValue((byte[])new_value, i);
}
return flatten_values;
    ";

    public ConvertByTeaArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConvertByTeaArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BYTEA[]") },
            ReturnType = "BYTEA[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bytea-null-1array", "ConvertByteaArray1", "ARRAY['Brick Abode is nice!'::BYTEA, 'Test 1!'::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]", "= ARRAY['\\x427269636b2041626f6465206973206e69636521'::BYTEA, '\\x54657374203121'::BYTEA, null::BYTEA, '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConvertByTeaArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
