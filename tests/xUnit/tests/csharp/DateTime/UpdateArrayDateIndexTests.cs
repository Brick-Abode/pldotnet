
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayDateIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
dates.SetValue(desired, arrayInteger);
return dates;
    ";

    public UpdateArrayDateIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayDateIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("dates", "DATE[]"), new FunctionArgument("desired", "DATE"), new FunctionArgument("index", "integer[]") },
            ReturnType = "DATE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-date-1array", "updateArrayDateIndex1", "ARRAY[DATE 'Oct-14-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022'], DATE 'Nov-18-2022', ARRAY[2]", "= ARRAY[DATE 'Oct-14-2022', DATE 'Oct-15-2022', DATE 'Nov-18-2022', DATE 'Oct-16-2022']" },
        new object[] { "c#-date-2array", "updateArrayDateIndex2", "ARRAY[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022', ARRAY[1, 0]", "= ARRAY[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [DATE 'Nov-18-2022', DATE 'Oct-16-2022']]" },
        new object[] { "c#-date-null-2array-arraynull", "updateArrayDateIndex3", "ARRAY[[null::date, null::date], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022', ARRAY[1, 0]", "= ARRAY[[null::date, null::date], [DATE 'Nov-18-2022', DATE 'Oct-16-2022']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayDateIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
