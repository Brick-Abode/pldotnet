using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnTextArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnTextArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnTextArrayFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("texts", "text[]") }, ReturnType = "text[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-text-null-1array", "returnTextArrayFSharp1", "ARRAY['test1'::text, null::text, 'test string 2'::text, null::text]", "= ARRAY['test1'::text, null::text, 'test string 2'::text, null::text]" }, new object[] { "f#-text-null-2array-arraynull", "returnTextArrayFSharp2", "ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]]", "= ARRAY[[null::text, null::text], ['test1'::text, 'test string 2'::text]]" }, new object[] { "f#-text-null-3array-arraynull", "returnTextArrayFSharp3", "ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]]", "= ARRAY[[[null::text, null::text], [null::text, null::text]], [['test1'::text, 'test 2'::text], ['test 3  abc'::text, 'test4'::text]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnTextArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class ReturnTextArrayFsharpTestsFSharp : BaseReturnTextArrayFsharpTests
{
    protected override string FunctionBody => @"
texts
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}