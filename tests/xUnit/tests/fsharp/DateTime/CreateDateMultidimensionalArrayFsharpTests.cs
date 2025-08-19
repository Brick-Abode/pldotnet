using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateDateMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateDateMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateDateMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "DATE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-date-null-3array-arraynull", "CreateDateMultidimensionalArrayFSharp", "", "= ARRAY[[[DATE 'Dec-25-2022'::date]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class CreateDateMultidimensionalArrayFsharpTestsFSharp : BaseCreateDateMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let objects_value = DateOnly(2022,12,25)
let arr = Array.CreateInstance(typeof<DateOnly>, 1, 1, 1)
arr.SetValue(objects_value, 0, 0, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}