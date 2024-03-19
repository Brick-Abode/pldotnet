
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class UpdateInT8RangeIndexFsharpTests : PlDotNetTest
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

    public UpdateInT8RangeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateInT8RangeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT8RANGE[]"), new FunctionArgument("b", "INT8RANGE") },
            ReturnType = "INT8RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int8range-null-1array", "updateInt8RangeIndexFSharp1", "ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE], '[6,)'::INT8RANGE", "= ARRAY['[6,)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]" },
        new object[] { "f#-int8range-null-2array", "updateInt8RangeIndexFSharp2", "ARRAY[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE", "= ARRAY[['[6,)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]" },
        new object[] { "f#-int8range-null-3array", "updateInt8RangeIndexFSharp3", "ARRAY[[['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]], '[6,)'::INT8RANGE", "= ARRAY[[['[6,)'::INT8RANGE, '(,6)'::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]]" },
        new object[] { "f#-int8range-null-2array-arraynull", "updateInt8RangeIndexFSharp4", "ARRAY[[null::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]], '[6,)'::INT8RANGE", "= ARRAY[['[6,)'::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]" },
        new object[] { "f#-int8range-null-3array-arraynull", "updateInt8RangeIndexFSharp5", "ARRAY[[[null::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]], '[6,)'::INT8RANGE", "= ARRAY[[['[6,)'::INT8RANGE, null::INT8RANGE], [null::INT8RANGE, '[,)'::INT8RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateInT8RangeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
