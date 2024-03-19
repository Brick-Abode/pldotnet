
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class CreateBooleanMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
bool?[, ,] boolean_three_dimensional = new bool?[2, 2, 2] {{{true, false}, {null, null}}, {{false, false}, {true, null}}};
return boolean_three_dimensional;
    ";

    public CreateBooleanMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBooleanMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "boolean[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bool-null-3array-arraynull", "CreateBooleanMultidimensionalArray", "", "= ARRAY[[[true, false], [null::boolean, null::boolean]], [[false, false], [true, null::boolean]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBooleanMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
