
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class UpdateArrayNetMaskIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayNetMaskIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayNetMaskIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INET[]"), new FunctionArgument("desired", "INET"), new FunctionArgument("index", "integer[]") },
            ReturnType = "INET[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-inet-1array", "updateArrayNetMaskIndex1", "ARRAY[INET '192.168.0.1/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24'], INET '192.168.0.120/24', ARRAY[2]", "= ARRAY[INET '192.168.0.1/24', INET '192.170.0.1/24', INET '192.168.0.120/24', INET '170.168.0.1/24']" },
        new object[] { "c#-inet-2array", "updateArrayNetMaskIndex2", "ARRAY[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24', ARRAY[1, 0]", "= ARRAY[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [INET '192.168.0.120/24', INET '170.168.0.1/24']]" },
        new object[] { "c#-inet-null-2array-arraynull", "updateArrayNetMaskIndex3", "ARRAY[[null::INET, null::INET], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24', ARRAY[1, 0]", "= ARRAY[[null::INET, null::INET], [INET '192.168.0.120/24', INET '170.168.0.1/24']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayNetMaskIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
