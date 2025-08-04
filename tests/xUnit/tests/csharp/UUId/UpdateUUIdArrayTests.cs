using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateUUIdArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateUUIdArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateUUIdArray", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "UUID[]") }, ReturnType = "UUID[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-uuid-null-1array", "updateUUIDArray1", "ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]", "= ARRAY['aaaaaaaa-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'aaaaaaaa-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'aaaaaaaa-9c0b-4ef8-9b6a-0242ac120002'::UUID]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateUUIdArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "UUId")]
public class UpdateUUIdArrayTestsCSharp : BaseUpdateUUIdArrayTests
{
    protected override string FunctionBody => @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i).ToString();
    Guid new_value = new Guid(""aaaaaaaa"" + orig_value.Substring(8));

    flatten_values.SetValue((Guid)new_value, i);
}
return flatten_values;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}