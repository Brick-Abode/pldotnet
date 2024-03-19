using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Json")]
public class GetJsonMultiDimensionArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string objects_value = ""{\""type\"": \""json\"", \""action\"": \""multidimensional test\""}"";
string?[, ,] three_dimensional_array = new string?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public GetJsonMultiDimensionArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "GetJsonMultiDimensionArray",
            Arguments = new List<FunctionArgument> { },
            ReturnType = "JSON[][][]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

   public static object[][] TestCases()
{
    return new object[][]
    {
        new object[] { 
            "c#-json-null-3array-arraynull", 
            "GetJsonMultidimensionArray1", 
            "", 
            "= ARRAY[[['{\"type\": \"json\", \"action\": \"multidimensional test\"}', '{\"type\": \"json\", \"action\": \"multidimensional test\"}'::JSON], [null::JSON, null::JSON]], [['{\"type\": \"json\", \"action\": \"multidimensional test\"}'::JSON, null::JSON], ['{\"type\": \"json\", \"action\": \"multidimensional test\"}'::JSON, '{\"type\": \"json\", \"action\": \"multidimensional test\"}'::JSON]]]"
        }
    };
}

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetJsonMultiDimensionArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
