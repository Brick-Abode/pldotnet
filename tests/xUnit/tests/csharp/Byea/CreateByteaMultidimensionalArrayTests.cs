
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Byea")]
public class CreateByTeaMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
byte[] objects_value = new byte[] { 0x92, 0x83, 0x74, 0x65, 0x56, 0x47, 0x38 };
byte[]?[, ,] three_dimensional_array = new byte[]?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateByTeaMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateByTeaMultidimensionalArray",
            Arguments = new List<FunctionArgument> { },
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
            new object[] { "c#-bytea-null-3array-arraynull", "CreateByteaMultidimensionalArray1", "", "= ARRAY[[['\\x92837465564738'::BYTEA, '\\x92837465564738'::BYTEA], [null::BYTEA, null::BYTEA]], [['\\x92837465564738'::BYTEA, null::BYTEA], ['\\x92837465564738'::BYTEA, '\\x92837465564738'::BYTEA]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateByTeaMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
