
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class UpdateInT4RangeIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateInT4RangeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateInT4RangeIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INT4RANGE[]"), new FunctionArgument("desired", "INT4RANGE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-int4range-null-1array", "updateInt4RangeIndex1", "ARRAY['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE], '[6,)'::INT4RANGE, ARRAY[2]", "= ARRAY['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE, '[6,)'::INT4RANGE, '[,)'::INT4RANGE]" },
        new object[] { "c#-int4range-null-2array", "updateInt4RangeIndex2", "ARRAY[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE, ARRAY[1, 0]", "= ARRAY[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], ['[6,)'::INT4RANGE, '[,)'::INT4RANGE]]" },
        new object[] { "c#-int4range-null-2array-arraynull", "updateInt4RangeIndex3", "ARRAY[[null::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE, ARRAY[1, 0]", "= ARRAY[[null::INT4RANGE, null::INT4RANGE], ['[6,)'::INT4RANGE, '[,)'::INT4RANGE]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateInT4RangeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
