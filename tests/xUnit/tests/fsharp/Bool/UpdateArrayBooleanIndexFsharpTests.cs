
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Bool")]
public class UpdateArrayBooleanIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
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

    public UpdateArrayBooleanIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayBooleanIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean[]"), new FunctionArgument("b", "boolean") },
            ReturnType = "boolean[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-bool-1array", "updateArrayBooleanIndexFSharp1", "ARRAY[true, false, true], false", "= ARRAY[false, false, true]" },
        new object[] { "f#-bool-2array", "updateArrayBooleanIndexFSharp2", "ARRAY[[true, false], [true, false]], false", "= ARRAY[[false, false], [true, false]]" },
        new object[] { "f#-bool-2array", "updateArrayBooleanIndexFSharp3", "ARRAY[[[true, false], [true, false]]], false", "= ARRAY[[[false, false], [true, false]]]" },
        new object[] { "f#-bool-null-1array", "updateArrayBooleanIndexFSharp4", "ARRAY[null::boolean, false, true], true", "= ARRAY[true, false, true]" },
        new object[] { "f#-bool-null-2array", "updateArrayBooleanIndexFSharp5", "ARRAY[[null::boolean, false], [null::boolean, false]], false", "= ARRAY[[false, false], [null::boolean, false]]" },
        new object[] { "f#-bool-null-2array", "updateArrayBooleanIndexFSharp6", "ARRAY[[[null::boolean, false], [null::boolean, false]]], false", "= ARRAY[[[false, false], [null::boolean, false]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayBooleanIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
