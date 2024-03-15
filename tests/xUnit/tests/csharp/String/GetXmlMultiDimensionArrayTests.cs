
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class GetXmlMultiDimensionArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string objects_value = ""<?xml version=\""1.0\"" encoding=\""utf-8\""?><title>Hello, World!</title>"";
string?[, ,] three_dimensional_array = new string?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public GetXmlMultiDimensionArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "GetXmlMultiDimensionArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "XML[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

  public static object[][] TestCases()
{
    return new object[][]
    {
        new object[]
        {
            "c#-xml-null-3array-arraynull",
            "GetXMLMultidimensionArray",
            "",
            "= ARRAY[[['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML], [null::XML, null::XML]], [['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML, null::XML], ['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML]]]::TEXT"
        }
    };
}

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetXmlMultiDimensionArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
