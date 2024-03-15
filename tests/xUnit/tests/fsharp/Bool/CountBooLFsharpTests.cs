
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class CountBooLFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let flatten_booleans = Array.CreateInstance(typeof<Object>, booleans.Length)
ArrayManipulation.FlatArray(booleans, ref flatten_booleans) |> ignore
let mutable count = 0
for i = 0 to flatten_booleans.Length - 1 do
    if System.Object.ReferenceEquals(flatten_booleans.GetValue(i), null) then
        ()
    else if flatten_booleans.GetValue(i).Equals(desired) then
        count <- count + 1
count
    ";

    public CountBooLFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CountBooLFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("booleans", "boolean[]"), new FunctionArgument("desired", "boolean") },
            ReturnType = "Integer",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bool-null-1array", "countBoolFSharp1", "ARRAY[true, true, false, true, null::boolean], true", "= integer '3'" },
        new object[] { "f#-bool-null-1array", "countBoolFSharp2", "ARRAY[true, true, false, true, null::boolean], false", "= integer '1'" },
        new object[] { "f#-bool-null-2array", "countBoolFSharp3", "ARRAY[[true, null::boolean, true], [true, false, null::boolean]], true", "= integer '3'" },
        new object[] { "f#-bool-null-2array", "countBoolFSharp4", "ARRAY[[true, null::boolean, true], [true, false, null::boolean]], false", "= integer '1'" },
        new object[] { "f#-bool-null-3array", "countBoolFSharp5", "ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], true", "= integer '5'" },
        new object[] { "f#-bool-null-3array", "countBoolFSharp6", "ARRAY[[[true, true, null::boolean], [true, null::boolean, false]], [[null::boolean, true, false], [true, null::boolean, false]]], false", "= integer '3'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCountBooLFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
