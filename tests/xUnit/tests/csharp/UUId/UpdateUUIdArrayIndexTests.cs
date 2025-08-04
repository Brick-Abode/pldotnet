using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateUUIdArrayIndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateUUIdArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateUUIdArrayIndex", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "UUID[]"), new FunctionArgument("desired", "UUID"), new FunctionArgument("index", "integer[]") }, ReturnType = "UUID[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-uuid-null-1array", "updateUUIDArrayIndex1", "ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[2]", "= ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]" }, new object[] { "c#-uuid-null-2array", "updateUUIDArrayIndex2", "ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[1,0]", "= ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], ['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]" }, new object[] { "c#-uuid-null-2array-arraynull", "updateUUIDArrayIndex3", "ARRAY[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, ARRAY[1,0]", "= ARRAY[[null::UUID, null::UUID], ['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateUUIdArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "UUId")]
public class UpdateUUIdArrayIndexTestsCSharp : BaseUpdateUUIdArrayIndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}