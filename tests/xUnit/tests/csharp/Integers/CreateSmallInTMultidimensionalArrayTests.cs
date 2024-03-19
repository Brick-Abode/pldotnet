
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class CreateSmallInTMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
short?[, ,] smallint_three_dimensional = new short?[2, 2, 2] {{{423, 536}, {null, null}}, {{8763, 15}, {943, 1003}}};
return smallint_three_dimensional;
    ";

    public CreateSmallInTMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateSmallInTMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "smallint[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int2-null-3array-arraynull", "CreateSmallIntMultidimensionalArray", "", "= ARRAY[[[423::smallint, 536::smallint], [null::smallint, null::smallint]], [[8763::smallint, 15::smallint], [943::smallint, 1003::smallint]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateSmallInTMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
