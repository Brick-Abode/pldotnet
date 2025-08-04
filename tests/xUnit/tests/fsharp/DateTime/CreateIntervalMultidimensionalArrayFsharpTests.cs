using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateIntervalMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateIntervalMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateIntervalMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "INTERVAL[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-interval-3array", "CreateIntervalMultidimensionalArrayFSharp", "", "= ARRAY[[[INTERVAL '10 months 33 days 15 minutes']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateIntervalMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateIntervalMultidimensionalArrayFsharpTestsFSharp : BaseCreateIntervalMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = NpgsqlInterval(10, 33, 900000000)
let arr = Array.CreateInstance(typeof<NpgsqlInterval>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}