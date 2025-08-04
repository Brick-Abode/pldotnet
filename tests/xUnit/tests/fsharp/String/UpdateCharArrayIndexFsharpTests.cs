using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateCharArrayIndexFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateCharArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateCharArrayIndexFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BPCHAR[]"), new FunctionArgument("desired", "BPCHAR"), new FunctionArgument("index", "integer[]") }, ReturnType = "BPCHAR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bpchar-null-1array", "updateCharArrayIndexFSharp1", "ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]", "= ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR]" }, new object[] { "f#-bpchar-null-2array-arraynull", "updateCharArrayIndexFSharp2", "ARRAY[[null::BPCHAR, null::BPCHAR], [null::BPCHAR, 'bye'::BPCHAR]], 'goodbye'::BPCHAR, ARRAY[1,0]", "= ARRAY[[null::BPCHAR, null::BPCHAR], ['goodbye'::BPCHAR, 'bye'::BPCHAR]]" }, new object[] { "f#-bpchar-null-1array", "updateCharArrayIndexFSharp3", "ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]", "= ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateCharArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class UpdateCharArrayIndexFsharpTestsFSharp : BaseUpdateCharArrayIndexFsharpTests
{
    protected override string FunctionBody => @"
let arrayInteger: int[]  = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}