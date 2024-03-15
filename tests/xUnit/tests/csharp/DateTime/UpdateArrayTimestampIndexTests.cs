
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimestampIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayTimestampIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimestampIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIMESTAMP[]"), new FunctionArgument("desired", "TIMESTAMP"), new FunctionArgument("index", "integer[]") },
            ReturnType = "TIMESTAMP[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamp-1array", "updateArrayTimestampIndex1", "ARRAY[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM'], TIMESTAMP '2025-10-19 10:23:54 PM', ARRAY[2]", "= ARRAY[TIMESTAMP '2004-10-19 10:23:54 PM', TIMESTAMP '2020-10-19 10:23:54 PM', TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2022-12-25 10:23:54 PM']" },
        new object[] { "c#-timestamp-null-2array-arraynull", "updateArrayTimestampIndex2", "ARRAY[[null::timestamp, null::timestamp], [null::timestamp, TIMESTAMP '2022-12-25 10:23:54 PM']], TIMESTAMP '2025-10-19 10:23:54 PM', ARRAY[1,0]", "= ARRAY[[null::timestamp, null::timestamp], [TIMESTAMP '2025-10-19 10:23:54 PM', TIMESTAMP '2022-12-25 10:23:54 PM']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimestampIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
