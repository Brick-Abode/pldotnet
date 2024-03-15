
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CreateCidrMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
(IPAddress Address, int Netmask) objects_value = (IPAddress.Parse(""127.123.54.0""), 24);
(IPAddress Address, int Netmask)?[, ,] three_dimensional_array = new (IPAddress Address, int Netmask)?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateCidrMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateCidrMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
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
            new object[] { "c#-cidr-3array", "CreateCIDRMultidimensionalArray", "", "= ARRAY[[[CIDR '127.123.54.0/24', CIDR '127.123.54.0/24'], [null::CIDR, null::CIDR]], [[CIDR '127.123.54.0/24', null::CIDR], [CIDR '127.123.54.0/24', CIDR '127.123.54.0/24']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateCidrMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
