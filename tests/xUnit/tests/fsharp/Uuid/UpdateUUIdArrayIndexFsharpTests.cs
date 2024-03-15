
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Uuid")]
public class UpdateUUIdArrayIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
    ";

    public UpdateUUIdArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateUUIdArrayIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID[]"), new FunctionArgument("b", "UUID") },
            ReturnType = "UUID[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-uuid-null-1array", "updateUUIDArrayIndexFSharp1", "ARRAY['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]" },
        new object[] { "f#-uuid-null-2array", "updateUUIDArrayIndexFSharp2", "ARRAY[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]" },
        new object[] { "f#-uuid-null-3array", "updateUUIDArrayIndexFSharp3", "ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY[[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]]" },
        new object[] { "f#-uuid-null-1array-arraynull", "updateUUIDArrayIndexFSharp4", "ARRAY[null::UUID, null::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID, null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]" },
        new object[] { "f#-uuid-null-2array-arraynull", "updateUUIDArrayIndexFSharp5", "ARRAY[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]" },
        new object[] { "f#-uuid-null-3array-arraynull", "updateUUIDArrayIndexFSharp6", "ARRAY[[[null::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]], 'fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID", "= ARRAY[[['fe1ebf99-9c4b-6ef8-9b87-02facc120002'::UUID, null::UUID], [null::UUID, 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateUUIdArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
