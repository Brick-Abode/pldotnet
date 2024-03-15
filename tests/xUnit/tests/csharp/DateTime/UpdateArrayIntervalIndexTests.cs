
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayIntervalIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayIntervalIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayIntervalIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INTERVAL[]"), new FunctionArgument("desired", "INTERVAL"), new FunctionArgument("index", "integer[]") },
            ReturnType = "INTERVAL[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-interval-1array", "updateArrayIntervalIndex1", "ARRAY[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', ARRAY[2]", "= ARRAY[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']" },
        new object[] { "c#-interval-2array", "updateArrayIntervalIndex2", "ARRAY[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', ARRAY[1, 0]", "= ARRAY[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]" },
        new object[] { "c#-interval-null-2array-arraynull", "updateArrayIntervalIndex3", "ARRAY[[null::interval, null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', ARRAY[1, 0]", "= ARRAY[[null::interval, null::interval], [INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayIntervalIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
