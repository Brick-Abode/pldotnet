
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class UpdateArrayCidrIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayCidrIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayCidrIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "CIDR[]"), new FunctionArgument("desired", "CIDR"), new FunctionArgument("index", "integer[]") },
            ReturnType = "CIDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-cidr-1array", "updateArrayCIDRIndex1", "ARRAY[CIDR '192.168/24', CIDR '170.168/24', null::cidr, CIDR '142.168/24'], CIDR '192.169/24', ARRAY[2]", "= ARRAY[CIDR '192.168/24', CIDR '170.168/24', CIDR '192.169/24', CIDR '142.168/24']" },
        new object[] { "c#-cidr-2array", "updateArrayCIDRIndex2", "ARRAY[[CIDR '192.168/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24', ARRAY[1, 0]", "= ARRAY[[CIDR '192.168/24', CIDR '170.168/24'], [CIDR '192.169/24', CIDR '142.168/24']]" },
        new object[] { "c#-cidr-null-2array-arraynull", "updateArrayCIDRIndex3", "ARRAY[[null::CIDR, null::CIDR], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24', ARRAY[1, 0]", "= ARRAY[[null::CIDR, null::CIDR], [CIDR '192.169/24', CIDR '142.168/24']]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayCidrIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
