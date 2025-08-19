using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateTimeMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateTimeMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateTimeMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "TIME[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-time-3array", "CreateTimeMultidimensionalArrayFSharp", "", "= ARRAY[[[TIME '10:33:55 AM']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTimeMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateTimeMultidimensionalArrayFsharpTestsFSharp : BaseCreateTimeMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = TimeOnly(10,33,55)
let arr = Array.CreateInstance(typeof<TimeOnly>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}