
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class CreateMoneyMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
decimal objects_value = 3720368547758.08M;
decimal?[, ,] three_dimensional_array = new decimal?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateMoneyMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateMoneyMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "MONEY[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-money-null-3array-arraynull", "CreateMoneyMultidimensionalArray1", "", "= ARRAY[[[3720368547758.08::MONEY, 3720368547758.08::MONEY], [null::MONEY, null::MONEY]], [[3720368547758.08::MONEY, null::MONEY], [3720368547758.08::MONEY, 3720368547758.08::MONEY]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateMoneyMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
