
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class UpdateInT8RangeIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateInT8RangeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateInT8RangeIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INT8RANGE[]"), new FunctionArgument("desired", "INT8RANGE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-int8range-null-1array", "updateInt8RangeIndex1", "ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE], '[6,)'::INT8RANGE, ARRAY[2]", "= ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, '[6,)'::INT8RANGE, '[,)'::INT8RANGE]" },
        new object[] { "c#-int8range-null-2array", "updateInt8RangeIndex2", "ARRAY[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE, ARRAY[1, 0]", "= ARRAY[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], ['[6,)'::INT8RANGE, '[,)'::INT8RANGE]]" },
        new object[] { "c#-int8range-null-2array-arraynull", "updateInt8RangeIndex3", "ARRAY[[null::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE, ARRAY[1, 0]", "= ARRAY[[null::INT8RANGE, null::INT8RANGE], ['[6,)'::INT8RANGE, '[,)'::INT8RANGE]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateInT8RangeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
