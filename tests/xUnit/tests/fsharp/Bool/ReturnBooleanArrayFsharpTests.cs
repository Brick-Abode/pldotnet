using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnBooleanArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnBooleanArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnBooleanArrayFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("booleans", "boolean[]") }, ReturnType = "boolean[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bool-null-1array", "returnBooleanArrayFSharp1", "ARRAY[true, null::boolean, false, false]", "= ARRAY[true, null::boolean, false, false]" }, new object[] { "f#-bool-null-2array-arraynull", "returnBooleanArrayFSharp2", "ARRAY[[true, false], [null::boolean, null::boolean]]", "= ARRAY[[true, false], [null::boolean, null::boolean]]" }, new object[] { "f#-bool-null-3array-arraynull", "returnBooleanArrayFSharp3", "ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]]", "= ARRAY[[[true, false], [null::boolean, null::boolean]], [[true, null::boolean], [true, null::boolean]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBooleanArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class ReturnBooleanArrayFsharpTestsFSharp : BaseReturnBooleanArrayFsharpTests
{
    protected override string FunctionBody => @"
booleans
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}