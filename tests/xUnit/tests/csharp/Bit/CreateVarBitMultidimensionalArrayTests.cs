
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class CreateVarBitMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
BitArray objects_value = new BitArray(new bool[8]{true, false, true, false, true, true, false, false});
BitArray?[, ,] three_dimensional_array = new BitArray?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateVarBitMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateVarBitMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "BIT VARYING[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varbit-null-3array-arraynull", "CreateVarbitMultidimensionalArray1", "", "= ARRAY[[['10101100'::BIT VARYING, '10101100'::BIT VARYING], [null::BIT VARYING, null::BIT VARYING]], [['10101100'::BIT VARYING, null::BIT VARYING], ['10101100'::BIT VARYING, '10101100'::BIT VARYING]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateVarBitMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
