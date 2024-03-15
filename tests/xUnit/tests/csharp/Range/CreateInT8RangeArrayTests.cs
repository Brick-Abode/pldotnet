
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class CreateInT8RangeArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlRange<long> objects_value = new NpgsqlRange<long>(64, true, false, 89, false, false);
NpgsqlRange<long>?[, ,] three_dimensional_array = new NpgsqlRange<long>?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateInT8RangeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateInT8RangeArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "INT8RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8range-null-3array-arraynull", "CreateInt8RangeArray1", "", "= ARRAY[[['[64,89)'::INT8RANGE,'[64,89)'::INT8RANGE], [null::INT8RANGE, null::INT8RANGE]], [['[64,89)'::INT8RANGE, null::INT8RANGE], ['[64,89)'::INT8RANGE, '[64,89)'::INT8RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInT8RangeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
