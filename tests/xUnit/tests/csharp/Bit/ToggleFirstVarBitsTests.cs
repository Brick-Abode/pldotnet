using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseToggleFirstVarBitsTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseToggleFirstVarBitsTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ToggleFirstVarBits", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array BIT", "VARYING[]") }, ReturnType = "BIT VARYING[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varbit-null-1array", "ToggleFirstVarbits1", "ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '001001'::BIT VARYING]", "= ARRAY['0010101101101'::BIT VARYING, '001011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestToggleFirstVarBits(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class ToggleFirstVarBitsTestsCSharp : BaseToggleFirstVarBitsTests
{
    protected override string FunctionBody => @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    BitArray orig_value = (BitArray)flatten_values.GetValue(i);
    BitArray new_value = orig_value;
    new_value[0] = new_value[0] ? false : true;

    flatten_values.SetValue((BitArray)new_value, i);
}
return flatten_values;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}