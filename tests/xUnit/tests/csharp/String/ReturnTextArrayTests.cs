
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class ReturnTextArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return texts;
    ";

    public ReturnTextArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnTextArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("texts", "text[]") },
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
            new object[] { "c#-text-null-1array", "returnTextArray1", "ARRAY['test1'::text, null::text, 'test string 2'::text, null::text]", "= ARRAY['test1'::text, null::text, 'test string 2'::text, null::text]" },
        new object[] { "c#-text-null-2array-arraynull", "returnTextArray2", "ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]]", "= ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]]" },
        new object[] { "c#-text-null-3array-arraynull", "returnTextArray3", "ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]]", "= ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnTextArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
