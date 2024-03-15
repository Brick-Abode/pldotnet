
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class UpdateVarBitArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateVarBitArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateVarBitArrayIndex",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("values_array BIT", "VARYING[]"),
                new FunctionArgument("desired BIT", "VARYING"),
                new FunctionArgument("index", "integer[]")
            },
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
            new object[] { "c#-varbit-null-1array", "updateVarbitArrayIndex1", "ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, null::BIT VARYING, '101001'::BIT VARYING], '1111111001111'::BIT VARYING, ARRAY[2]", "= ARRAY['1010101101101'::BIT VARYING, '101011101'::BIT VARYING, '1111111001111'::BIT VARYING, '101001'::BIT VARYING]" },
            new object[] { "c#-varbit-null-2array", "updateVarbitArrayIndex2", "ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING, ARRAY[1, 0]", "= ARRAY[['1010101101101'::BIT VARYING, '101011101'::BIT VARYING], ['1111111001111'::BIT VARYING, '101001'::BIT VARYING]]" },
            new object[] { "c#-varbit-null-2array-arraynull", "updateVarbitArrayIndex3", "ARRAY[[null::BIT VARYING, null::BIT VARYING], [null::BIT VARYING, '101001'::BIT VARYING]], '1111111001111'::BIT VARYING, ARRAY[1, 0]", "= ARRAY[[null::BIT VARYING, null::BIT VARYING], ['1111111001111'::BIT VARYING, '101001'::BIT VARYING]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateVarBitArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
