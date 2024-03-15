
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class UpdateArrayMacAddressIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayMacAddressIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayMacAddressIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR[]"), new FunctionArgument("desired", "MACADDR"), new FunctionArgument("index", "integer[]") },
            ReturnType = "MACADDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr-1array", "updateArrayMacAddressIndex1", "ARRAY[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03'], MACADDR 'd1-00-2b-01-02-03', ARRAY[2]", "= ARRAY[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', MACADDR 'd1-00-2b-01-02-03', MACADDR 'a8-00-2b-01-02-03']" },
        new object[] { "c#-macaddr-2array", "updateArrayMacAddressIndex2", "ARRAY[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03', ARRAY[1, 0]", "= ARRAY[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [MACADDR 'd1-00-2b-01-02-03', MACADDR 'a8-00-2b-01-02-03']]" },
        new object[] { "c#-macaddr-null-2array-arraynull", "updateArrayMacAddressIndex3", "ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03', ARRAY[1, 0]", "= ARRAY[[null::macaddr, null::macaddr], [MACADDR 'd1-00-2b-01-02-03', MACADDR 'a8-00-2b-01-02-03']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayMacAddressIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
