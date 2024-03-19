
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Bit")]
public class UpdateBitArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateBitArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateBitArrayIndex",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("values_array", "BIT(8)[]"),
                new FunctionArgument("desired", "BIT(8)"),
                new FunctionArgument("index", "integer[]"),
            },
            ReturnType = "BIT(8)[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bit-null-1array", "updateBitArrayIndex1", "ARRAY['10101001'::BIT(8), '10101101'::BIT(8), null::BIT(8), '11101001'::BIT(8)], '11111111'::BIT(8), ARRAY[2]", "= ARRAY['10101001'::BIT(8), '10101101'::BIT(8), '11111111'::BIT(8), '11101001'::BIT(8)]" },
            new object[] { "c#-bit-null-2array", "updateBitArrayIndex2", "ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8), ARRAY[1,0]", "= ARRAY[['10101001'::BIT(8), '10101101'::BIT(8)], ['11111111'::BIT(8), '11101001'::BIT(8)]]" },
            new object[] { "c#-bit-null-2array-arraynull", "updateBitArrayIndex3", "ARRAY[[null::BIT(8), null::BIT(8)], [null::BIT(8), '11101001'::BIT(8)]], '11111111'::BIT(8), ARRAY[1,0]", "= ARRAY[[null::BIT(8), null::BIT(8)], ['11111111'::BIT(8), '11101001'::BIT(8)]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateBitArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
