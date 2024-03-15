
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class UpdateArrayTextIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arrayInteger: int[] = index.Cast<int>().ToArray()
texts.SetValue(desired, arrayInteger)
texts
    ";

    public UpdateArrayTextIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayTextIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("texts", "text[]"), new FunctionArgument("desired", "text"), new FunctionArgument("index", "integer[]") },
            ReturnType = "text[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-text-null-1array", "updateArrayTextIndexFSharp1", "ARRAY['test1'::text, null::text, ' test string 2'::text, null::text], 'test updated', ARRAY[1]", "= ARRAY['test1'::text, 'test updated'::text, ' test string 2'::text, null::text]" },
        new object[] { "f#-text-null-3array", "updateArrayTextIndexFSharp2", "ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, ' is'::text], [' this'::text, ' text'::text, ' good?'::text]]], 'test updated', ARRAY[1, 0, 2]", "= ARRAY[[['test1'::text, null::text, ' appended'::text], [' to'::text, ' another'::text, ' text:'::text]], [[' test string 2,'::text, null::text, 'test updated'::text], [' this'::text, ' text'::text, ' good?'::text]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTextIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
