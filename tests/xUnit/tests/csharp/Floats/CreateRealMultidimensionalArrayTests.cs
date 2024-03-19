
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class CreateRealMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
float?[, ,] float_three_dimensional = new float?[2, 2, 2] {{{1.24323f, 3.42345f}, {null, null}}, {{9.32425f, 8.11134f}, {10.32145f, 16.14256f}}};
return float_three_dimensional;
    ";

    public CreateRealMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateRealMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "real[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float4-null-3array-arraynull", "CreateRealMultidimensionalArray", "", "= ARRAY[[[1.24323::real, 3.42345::real], [null::real, null::real]], [[9.32425::real, 8.11134::real], [10.32145::real, 16.14256::real]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateRealMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
