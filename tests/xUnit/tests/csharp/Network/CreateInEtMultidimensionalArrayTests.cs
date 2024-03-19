
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CreateInEtMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
(IPAddress Address, int Netmask) objects_value = (IPAddress.Parse(""127.0.0.1""), 21);
(IPAddress Address, int Netmask)?[, ,] three_dimensional_array = new (IPAddress Address, int Netmask)?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateInEtMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateInEtMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
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
            new object[] { "c#-inet-3array", "CreateInetMultidimensionalArray", "", "= ARRAY[[[INET '127.0.0.1/21', INET '127.0.0.1/21'], [null::INET, null::INET]], [[INET '127.0.0.1/21', null::INET], [INET '127.0.0.1/21', INET '127.0.0.1/21']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInEtMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
