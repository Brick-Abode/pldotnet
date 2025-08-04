using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateBoxMultidimensionalArrayFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateBoxMultidimensionalArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateBoxMultidimensionalArrayFsharp", Arguments = new List<FunctionArgument> { }, ReturnType = "BOX[]", Body = FunctionBody, Language = Language, IsStrict = true, CastFunctionAs = "TEXT" };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-box-null-3array-arraynull", "CreateBoxMultidimensionalArrayFSharp1", "", "= '{{{(3,4.75),(1.5,2.75)};{(0,0),(0,0)};{(0,0),(0,0)}};{{(0,0),(0,0)};{(3,4.75),(1.5,2.75)};{(0,0),(0,0)}};{{(0,0),(0,0)};{(0,0),(0,0)};{(3,4.75),(1.5,2.75)}}}'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBoxMultidimensionalArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class CreateBoxMultidimensionalArrayFsharpTestsFSharp : BaseCreateBoxMultidimensionalArrayFsharpTests
{
    protected override string FunctionBody => @"
let arr = Array.CreateInstance(typeof<NpgsqlBox>, 3, 3, 1)
let objects_value = NpgsqlBox(NpgsqlPoint(1.5, 2.75), NpgsqlPoint(3.0, 4.75))
arr.SetValue(objects_value, 0, 0, 0)
arr.SetValue(objects_value, 1, 1, 0)
arr.SetValue(objects_value, 2, 2, 0)
arr
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}