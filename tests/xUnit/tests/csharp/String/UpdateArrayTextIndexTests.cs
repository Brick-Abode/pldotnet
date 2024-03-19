
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class UpdateArrayTextIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
texts.SetValue(desired, arrayInteger);
return texts;
    ";

    public UpdateArrayTextIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTextIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("texts", "text[]"), new FunctionArgument("desired", "text"), new FunctionArgument("index", "integer[]") },
            ReturnType = "text[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-text-null-1array", "updateArrayTextIndex1", "ARRAY['test1'::text, null::text, ' test string 2'::text, null::text], 'test updated', ARRAY[1]", "= ARRAY['test1'::text, 'test updated'::text, ' test string 2'::text, null::text]" },
        new object[] { "c#-text-null-3array", "updateArrayTextIndex2", "ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, ' is'::text], [' this'::text, ' text'::text, ' good?'::text]]], 'test updated', ARRAY[1, 0, 2]", "= ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, 'test updated'::text], [' this'::text, ' text'::text, ' good?'::text]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTextIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
