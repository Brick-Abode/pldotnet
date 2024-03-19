
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ModifyXmlFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let mutable new_xml: string = """"
    if System.Object.ReferenceEquals(a, null) ||a.Equals("""") then
        new_xml <- ""<?xml version=\""1.0\"" encoding=\""utf-8\""?><title>Hello, World, it was null!</title>""
    else
        new_xml <- a
    new_xml <- (new_xml.Replace(""Hello"", ""Goodbye"")).Replace(""World"", ""beautiful World"")

    new_xml
    ";

    public ModifyXmlFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyXmlFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "XML") },
            ReturnType = "XML",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
   new object[] {
                "f#-xml",
                "modifyXmlFSharp1",
                "'<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Hello, World!</title>'::XML",
                " = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, beautiful World!</title>'::XML::text"
            },
            new object[] {
                "f#-xml",
                "modifyXmlFSharp2",
                "''::XML",
                " = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, beautiful World, it was null!</title>'::XML::text"
            },
            new object[] {
                "f#-xml-null",
                "modifyXmlFSharp3",
                "NULL::XML",
                " = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>Goodbye, beautiful World, it was null!</title>'::XML::text"
            },        
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyXmlFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
