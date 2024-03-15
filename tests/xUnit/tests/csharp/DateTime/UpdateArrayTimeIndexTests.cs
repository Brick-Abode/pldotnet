
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimeIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayTimeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTimeIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIME[]"), new FunctionArgument("desired", "TIME"), new FunctionArgument("index", "integer[]") },
            ReturnType = "TIME[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-time-1array", "updateArrayTimeIndex1", "ARRAY[TIME '05:30 PM', TIME '06:30 PM', null::time, TIME '09:30 AM'], TIME '5:45 AM', ARRAY[2]", "= ARRAY[TIME '05:30 PM', TIME '06:30 PM', TIME '05:45 AM', TIME '09:30 AM']" },
        new object[] { "c#-time-2array", "updateArrayTimeIndex2", "ARRAY[[TIME '05:30 PM', TIME '06:30 PM'], [null::time, TIME '09:30 AM']], TIME '5:45 AM', ARRAY[1, 0]", "= ARRAY[[TIME '05:30 PM', TIME '06:30 PM'], [TIME '05:45 AM', TIME '09:30 AM']]" },
        new object[] { "c#-time-null-2array-arraynull", "updateArrayTimeIndex3", "ARRAY[[null::TIME, null::TIME], [null::time, TIME '09:30 AM']], TIME '5:45 AM', ARRAY[1, 0]", "= ARRAY[[null::TIME, null::TIME], [TIME '05:45 AM', TIME '09:30 AM']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
