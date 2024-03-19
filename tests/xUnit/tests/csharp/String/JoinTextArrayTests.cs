
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class JoinTextArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Array flatten_texts = Array.CreateInstance(typeof(object), texts.Length);
ArrayManipulation.FlatArray(texts, ref flatten_texts);
string result = """";
for(int i = 0; i < flatten_texts.Length; i++)
{
    if (flatten_texts.GetValue(i) == null)
        continue;
    result = (string)(result + (string)flatten_texts.GetValue(i));
}
return result;
    ";

    public JoinTextArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "JoinTextArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("texts", "text[]") },
            ReturnType = "text",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-text-null-1array", "JoinTextArray1", "ARRAY['test1'::text, null::text, ' test string 2'::text, null::text]", "= 'test1 test string 2'" },
        new object[] { "c#-text-null-3array", "JoinTextArray2", "ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, ' is'::text], [' this'::text, ' text'::text, ' good?'::text]]]", "= 'test1 appended to another text: test string 2, is this text good?'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestJoinTextArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
