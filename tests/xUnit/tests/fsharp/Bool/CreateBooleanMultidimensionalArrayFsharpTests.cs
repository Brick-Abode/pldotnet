using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateBooleanMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateBooleanMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateBooleanMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "boolean[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-bool-null-3array-arraynull", "CreateBooleanMultidimensionalArrayFSharp", "", "= ARRAY[[true, false, false], [false, true, false], [false, false, true]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBooleanMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class CreateBooleanMultidimensionalArrayFsharpTestsFSharp : BaseCreateBooleanMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let arr = Array.CreateInstance(typeof<bool>, 3, 3)
arr.SetValue(true, 0, 0)
arr.SetValue(true, 1, 1)
arr.SetValue(true, 2, 2)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}