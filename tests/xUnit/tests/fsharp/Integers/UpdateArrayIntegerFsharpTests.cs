using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayIntegerFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayIntegerFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayIntegerFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int8[]"), new FunctionArgument("b", "int8") }, ReturnType = "int8[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-int8-1array", "updateArrayIntegerFSharp1", "ARRAY[null::int8, null::int8, 2047483647::int8, 304325::int8], '250'::int2", "= ARRAY['250'::int2, null::int8, 2047483647::int8, 304325::int8]" }, new object[] { "f#-int8-2array", "updateArrayIntegerFSharp2", "ARRAY[[null::int8, null::int8], [2047483647::int8, 304325::int8]], '250'::int2", "= ARRAY[['250'::int2, null::int8], [2047483647::int8, 304325::int8]]" }, new object[] { "f#-int8-3array", "updateArrayIntegerFSharp3", "ARRAY[[[null::int8, null::int8], [2047483647::int8, 304325::int8]]], '250'::int2", "= ARRAY[[['250'::int2, null::int8], [2047483647::int8, 304325::int8]]]" }, new object[] { "f#-int8-null-1array-arraynull", "updateArrayIntegerFSharp4", "ARRAY[null::int8, null::int8, null::int8, 304325::int8], '250'::int2", "= ARRAY['250'::int2, null::int8, null::int8, 304325::int8]" }, new object[] { "f#-int8-null-2array-arraynull", "updateArrayIntegerFSharp5", "ARRAY[[null::int8, null::int8], [null::int8, 304325::int8]], '250'::int2", "= ARRAY[['250'::int2, null::int8], [null::int8, 304325::int8]]" }, new object[] { "f#-int8-null-3array-arraynull", "updateArrayIntegerFSharp6", "ARRAY[[[null::int8, null::int8], [null::int8, 304325::int8]]], '250'::int2", "= ARRAY[[['250'::int2, null::int8], [null::int8, 304325::int8]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayIntegerFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class UpdateArrayIntegerFsharpTestsFSharp : BaseUpdateArrayIntegerFsharpTests
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