using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseGetJsonMultiDimensionArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseGetJsonMultiDimensionArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "GetJsonMultiDimensionArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "JSON[][][]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-json-null-3array-arraynull", "GetJsonMultidimensionArrayFsharp1", "", "= ARRAY[[['{\"type\": \"json\", \"action\": \"multidimensional test\"}'::JSON]]]" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestGetJsonMultiDimensionArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Json")]
public class GetJsonMultiDimensionArrayFsharpTestsFSharp : BaseGetJsonMultiDimensionArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = ""{\""type\"": \""json\"", \""action\"": \""multidimensional test\""}""
let arr = Array.CreateInstance(typeof<string>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}