using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReplaceXmlSwordTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReplaceXmlSwordTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReplaceXmlSword", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "XML[]") }, ReturnType = "XML[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-xml-null-1array", "ReplaceXMLsWord1", "ARRAY['Hello Mikael'::XML, 'Hello Rosicley'::XML, null::XML, 'Hello Todd'::XML]", "= ARRAY['Goodbye Mikael'::XML, 'Goodbye Rosicley'::XML, null::XML, 'Goodbye Todd'::XML]::TEXT" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReplaceXmlSword(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ReplaceXmlSwordTestsCSharp : BaseReplaceXmlSwordTests
{
    protected override string FunctionBody => @"
Array flatten_values = Array.CreateInstance(typeof(object), values_array.Length);
ArrayManipulation.FlatArray(values_array, ref flatten_values);
for(int i = 0; i < flatten_values.Length; i++)
{
    if (flatten_values.GetValue(i) == null)
        continue;

    string orig_value = (string)flatten_values.GetValue(i);
    string new_value = orig_value.Replace(""Hello"", ""Goodbye"");

    flatten_values.SetValue((string)new_value, i);
}
return flatten_values;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}