
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ModifyXmlTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
        a = ""<?xml version=\""1.0\"" encoding=\""utf-8\""?><title>Hello, World, it was null!</title>"";

    string new_xml = ((string)a).Replace(""Hello"", ""Goodbye"");
    new_xml = ((string)new_xml).Replace(""World"", ""beautiful World"");
    return new_xml;
    ";

    public ModifyXmlTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyXml",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "XML") },
            ReturnType = "XML",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "c#-xml",
                "modifyXml1",
                "'<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML",
                " = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, beautiful World!</title>'::XML::text"
            },
            new object[] {
                "c#-xml-null",
                "modifyXml2",
                "NULL::XML",
                " = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, beautiful World, it was null!</title>'::XML::text"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyXml(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
