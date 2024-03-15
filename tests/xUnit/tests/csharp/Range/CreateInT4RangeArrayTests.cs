
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class CreateInT4RangeArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlRange<int> objects_value = new NpgsqlRange<int>(64, true, false, 89, false, false);
NpgsqlRange<int>?[, ,] three_dimensional_array = new NpgsqlRange<int>?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateInT4RangeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateInT4RangeArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "INT4RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4range-null-3array-arraynull", "CreateInt4RangeArray1", "", "= ARRAY[[['[64,89)'::INT4RANGE,'[64,89)'::INT4RANGE], [null::INT4RANGE, null::INT4RANGE]], [['[64,89)'::INT4RANGE, null::INT4RANGE], ['[64,89)'::INT4RANGE, '[64,89)'::INT4RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInT4RangeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
