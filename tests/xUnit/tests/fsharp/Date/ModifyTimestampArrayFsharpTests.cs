
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Date")]
public class ModifyTimestampArrayFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
#line 1
    let dim = a.Rank
    let newValue =
        match b.HasValue with
        | true -> b.Value
        | false -> new DateTime(2022, 11, 15, 13, 23, 45)
    match dim with
    | 1 ->
        a.SetValue(newValue, 0) |> ignore
        a
    | 2 ->
        a.SetValue(newValue, 0, 0) |> ignore
        a
    | 3 ->
        a.SetValue(newValue, 0, 0, 0) |> ignore
        a
    | _ -> a
    ";

    public ModifyTimestampArrayFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyTimestampArrayFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TIMESTAMP[]"), new FunctionArgument("b", "TIMESTAMP") },
            ReturnType = "TIMESTAMP[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamp-1array-null", "modifyTimestampArrayFSharp1", "ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], NULL::TIMESTAMP", "= ARRAY['2022-11-15 13:23:45'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]" },
        new object[] { "f#-timestamp-1array-null", "modifyTimestampArrayFSharp2", "ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], '2023-01-01 12:12:12 PM'::TIMESTAMP", "= ARRAY['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]" },
        new object[] { "f#-timestamp-2array-null", "modifyTimestampArrayFSharp3", "ARRAY[['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP", "= ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]]" },
        new object[] { "f#-timestamp-2array-null", "modifyTimestampArrayFSharp4", "ARRAY[[NULL::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP", "= ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyTimestampArrayFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
