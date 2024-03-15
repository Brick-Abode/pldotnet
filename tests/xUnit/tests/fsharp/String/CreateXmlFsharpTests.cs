
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class CreateXmlFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
""<?xml version=\""1.0\"" encoding=\""utf-8\""?><title>"" + title.ToUpper() + ""</title><body><p>"" + p1 + ""</p><p>"" + p2 + ""</p></body>""
    ";

    public CreateXmlFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateXmlFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("title", "TEXT"), new FunctionArgument("p1", "TEXT"), new FunctionArgument("p2", "TEXT") },
            ReturnType = "XML",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
      new object[]
            {
                "f#-xml",
                "createXmlFSharp1",
                "'hello world'::TEXT, 'First paragraph'::TEXT, 'Second paragraph'::TEXT",
                "= '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>HELLO WORLD</title><body><p>First paragraph</p><p>Second paragraph</p></body>'::XML::TEXT"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateXmlFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
