
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class UpdateArrayMacAddress8IndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayMacAddress8IndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayMacAddress8Index",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR8[]"), new FunctionArgument("desired", "MACADDR8"), new FunctionArgument("index", "integer[]") },
            ReturnType = "MACADDR8[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr8-1array", "updateArrayMacAddress8Index1", "ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[2]", "= ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']" },
        new object[] { "c#-macaddr8-2array", "updateArrayMacAddress8Index2", "ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[1, 0]", "= ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" },
        new object[] { "c#-macaddr8-null-2array-arraynull", "updateArrayMacAddress8Index3", "ARRAY[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[1, 0]", "= ARRAY[[null::MACADDR8, null::MACADDR8], [MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayMacAddress8Index(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
