
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class CreateXmlTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string c = ""<?xml version=\""1.0\"" encoding=\""utf-8\""?>"";
    c += $""<title>{title.ToUpper()}</title>"";
    c += ""<body>"";
    c += $""<p>{p1}</p>"";
    c += $""<p>{p2}</p>"";
    c += ""</body>"";
    return c;
    ";

    public CreateXmlTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateXml",
            Arguments = new List<FunctionArgument> { new FunctionArgument("title", "TEXT"), new FunctionArgument("p1", "TEXT"), new FunctionArgument("p2", "TEXT") },
            ReturnType = "XML",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {"c#-xml","createXml","'hello world'::TEXT, 'First paragraph'::TEXT, 'Second paragraph'::TEXT"," = '<?xml version=\"1.0\" encoding=\"utf-8\"?><title>HELLO WORLD</title><body><p>First paragraph</p><p>Second paragraph</p></body>'::XML::TEXT"}
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateXml(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
