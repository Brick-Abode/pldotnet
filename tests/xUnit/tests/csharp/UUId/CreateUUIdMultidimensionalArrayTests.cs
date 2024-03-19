
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "UUId")]
public class CreateUUIdMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Guid objects_value = new Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"");
Guid?[, ,] three_dimensional_array = new Guid?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateUUIdMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateUUIdMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "UUID[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-uuid-null-3array-arraynull", "CreateUUIDMultidimensionalArray1", "", "= ARRAY[[['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID], [null::UUID, null::UUID]], [['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, null::UUID], ['a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateUUIdMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
