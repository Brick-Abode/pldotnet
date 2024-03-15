
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Range")]
public class UpdateInT4RangeIndexFsharpTests : PlDotNetTest
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

    public UpdateInT4RangeIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateInT4RangeIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT4RANGE[]"), new FunctionArgument("b", "INT4RANGE") },
            ReturnType = "INT4RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4range-null-1array", "updateInt4RangeIndexFSharp1", "ARRAY['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE], '[6,)'::INT4RANGE", "= ARRAY['[6,)'::INT4RANGE, '(,6)'::INT4RANGE, null::INT4RANGE, '[,)'::INT4RANGE]" },
        new object[] { "f#-int4range-null-2array", "updateInt4RangeIndexFSharp2", "ARRAY[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE", "= ARRAY[['[6,)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]" },
        new object[] { "f#-int4range-null-3array", "updateInt4RangeIndexFSharp3", "ARRAY[[['[2,6)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]], '[6,)'::INT4RANGE", "= ARRAY[[['[6,)'::INT4RANGE, '(,6)'::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]]" },
        new object[] { "f#-int4range-null-2array-arraynull", "updateInt4RangeIndexFSharp4", "ARRAY[[null::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]], '[6,)'::INT4RANGE", "= ARRAY[['[6,)'::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]" },
        new object[] { "f#-int4range-null-3array-arraynull", "updateInt4RangeIndexFSharp5", "ARRAY[[[null::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]], '[6,)'::INT4RANGE", "= ARRAY[[['[6,)'::INT4RANGE, null::INT4RANGE], [null::INT4RANGE, '[,)'::INT4RANGE]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateInT4RangeIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
