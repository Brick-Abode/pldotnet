
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class GetVarCharMultiDimensionArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string objects_value = ""Multiple dimensions"";
string?[, ,] three_dimensional_array = new string?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public GetVarCharMultiDimensionArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "GetVarCharMultiDimensionArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "VARCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varchar-null-3array-arraynull", "GetVarcharMultidimensionArray", "", "= ARRAY[[['Multiple dimensions'::VARCHAR, 'Multiple dimensions'::VARCHAR], [null::VARCHAR, null::VARCHAR]], [['Multiple dimensions'::VARCHAR, null::VARCHAR], ['Multiple dimensions'::VARCHAR, 'Multiple dimensions'::VARCHAR]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetVarCharMultiDimensionArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
