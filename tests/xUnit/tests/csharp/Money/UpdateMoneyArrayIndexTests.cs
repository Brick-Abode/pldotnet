
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Money")]
public class UpdateMoneyArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateMoneyArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateMoneyArrayIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MONEY[]"), new FunctionArgument("desired", "MONEY"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-money-null-1array", "updateMoneyArrayIndex1", "ARRAY['32500.0'::MONEY, '-500.4'::MONEY, null::MONEY, '900540.2'::MONEY], '1390540.2'::MONEY, ARRAY[2]", "= ARRAY['32500.0'::MONEY, '-500.4'::MONEY, '1390540.2'::MONEY, '900540.2'::MONEY]" },
        new object[] { "c#-money-null-2array-arraynull", "updateMoneyArrayIndex2", "ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]", "= ARRAY[['32500.0'::MONEY, '-500.4'::MONEY], ['1390540.2'::MONEY, null::MONEY]]" },
        new object[] { "c#-money-null-2array-arraynull", "updateMoneyArrayIndex3", "ARRAY[[null::MONEY, null::MONEY], [null::MONEY, null::MONEY]], '1390540.2'::MONEY, ARRAY[1,0]", "= ARRAY[[null::MONEY, null::MONEY], ['1390540.2'::MONEY, null::MONEY]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateMoneyArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
