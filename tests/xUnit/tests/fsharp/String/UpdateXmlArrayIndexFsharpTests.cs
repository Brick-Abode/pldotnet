
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class UpdateXmlArrayIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arrayInteger: int[] = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
    ";

    public UpdateXmlArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateXmlArrayIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "XML[]"), new FunctionArgument("desired", "XML"), new FunctionArgument("index", "integer[]") },
            ReturnType = "XML[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
     new object[] 
            { 
                "f#-xml-null-1array", 
                "updateXMLArrayIndex1", 
                "ARRAY['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Test 1!</title>'::XML, null::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, World!</title>'::XML], '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Writing tests!</title>'::XML, ARRAY[2]", 
                "= ARRAY['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Test 1!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Writing tests!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, World!</title>'::XML]::TEXT" 
            },
            new object[] 
            { 
                "f#-xml-null-2array-arraynull", 
                "updateXMLArrayIndex2", 
               "ARRAY[[null::XML, null::XML], [null::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, World!</title>'::XML]], '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Writing tests!</title>'::XML, ARRAY[1,0]",
                "= ARRAY[[null::XML, null::XML], ['<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Writing tests!</title>'::XML, '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, World!</title>'::XML]]::TEXT" 
            }        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateXmlArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
