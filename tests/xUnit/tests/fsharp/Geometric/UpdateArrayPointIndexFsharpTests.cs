using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayPointIndexFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayPointIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayPointIndexFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "point[]"), new FunctionArgument("b", "point") }, ReturnType = "point[]", Body = FunctionBody, Language = Language, IsStrict = true, CastFunctionAs = "TEXT", };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-point-null-1array", "updateArrayPointIndexFSharp1", "ARRAY[POINT(10.0,20.0), POINT(30.0,55.0), null::point, POINT(40.5,21.3)], POINT(31.43, 32.44)", "= CAST(ARRAY[POINT(31.43, 32.44), POINT(30.0,55.0), null::point, POINT(40.5,21.3)] AS TEXT)" }, new object[] { "f#-point-null-2array-arraynull", "updateArrayPointIndexFSharp2", "ARRAY[[null::point, null::point], [null::point, POINT(40.5,21.3)]], POINT(31.43, 32.44)", "= CAST(ARRAY[[POINT(31.43, 32.44), null::point], [null::point, POINT(40.5,21.3)]] AS TEXT)" }, new object[] { "f#-point-null-3array-arraynull", "updateArrayPointIndexFSharp3", "ARRAY[[[null::point, null::point], [null::point, POINT(40.5,21.3)]]], POINT(31.43, 32.44)", "= CAST(ARRAY[[[POINT(31.43, 32.44), null::point], [null::point, POINT(40.5,21.3)]]] AS TEXT)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPointIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayPointIndexFsharpTestsFSharp : BaseUpdateArrayPointIndexFsharpTests
{
    protected override string FunctionBody => @"
let dim = a.Rank
match dim with
| 1 ->
    a.SetValue(b, 0) |> ignore
    a
| 2 ->
    a.SetValue(b, 0, 0) |> ignore
    a
| 3 ->
    a.SetValue(b, 0, 0, 0) |> ignore
    a
| _ -> a
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}