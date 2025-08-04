using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateTimeTzMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateTimeTzMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateTimeTzMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "TIMETZ[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-timetz-3array", "CreateTimetzMultidimensionalArrayFSharp", "", "= ARRAY[[[TIMETZ '10:33:55+02:00']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimeTzMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateTimeTzMultidimensionalArrayFsharpTestsFSharp : BaseCreateTimeTzMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = DateTimeOffset(2022, 12, 25, 10, 33, 55, TimeSpan(2, 0, 0));
let arr = Array.CreateInstance(typeof<DateTimeOffset>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}