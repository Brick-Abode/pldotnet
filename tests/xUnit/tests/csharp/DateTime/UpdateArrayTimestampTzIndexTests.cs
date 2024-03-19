
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimestampTzIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayTimestampTzIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimestampTzIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array TIMESTAMP WITH TIME", "ZONE[]"), new FunctionArgument("desired TIMESTAMP WITH TIME", "ZONE"), new FunctionArgument("index", "integer[]") },
            ReturnType = "TIMESTAMP WITH TIME ZONE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamptz-1array", "updateArrayTimestamptzIndex1", "ARRAY[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05'], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', ARRAY[2]", "= ARRAY[TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', TIMESTAMP WITH TIME ZONE '2020-10-19 10:23:54 PM +03', TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']" },
        new object[] { "c#-timestamptz-null-2array-arraynull", "updateArrayTimestamptzIndex2", "ARRAY[[null::timestamptz, null::timestamptz], [null::timestamptz, TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']], TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', ARRAY[1,0]", "= ARRAY[[null::timestamptz, null::timestamptz], [TIMESTAMP WITH TIME ZONE '2025-10-19 10:23:54 PM -03', TIMESTAMP WITH TIME ZONE '2022-12-25 10:23:54 PM -05']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimestampTzIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
