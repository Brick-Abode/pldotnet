using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateInT8RangeArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateInT8RangeArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateInT8RangeArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "INT8RANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int8range-null-3array-arraynull", "CreateInt8RangeArrayFSharp1", "", "= ARRAY[[['[64,89)'::INT8RANGE]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateInT8RangeArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class CreateInT8RangeArrayFsharpTestsFSharp : BaseCreateInT8RangeArrayFsharpTests
{
    protected override string FunctionBody => @"
let arr = Array.CreateInstance(typeof<NpgsqlRange<int64>>, 1, 1, 1)
let objects_value = NpgsqlRange<int64>(64, true, false, 89, false, false)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}