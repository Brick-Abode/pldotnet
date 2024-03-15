
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayDateIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayDateIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayDateIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "DATE[]"), new FunctionArgument("b", "DATE") },
            ReturnType = "DATE[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-date-1array", "updateArrayDateIndexFSharp1", "ARRAY[DATE 'Oct-14-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022'], DATE 'Nov-18-2022'", "= ARRAY[DATE 'Nov-18-2022', DATE 'Oct-15-2022', null::date, DATE 'Oct-16-2022']" },
        new object[] { "f#-date-2array", "updateArrayDateIndexFSharp2", "ARRAY[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022'", "= ARRAY[[DATE 'Nov-18-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']]" },
        new object[] { "f#-date-3array", "updateArrayDateIndexFSharp3", "ARRAY[[[DATE 'Oct-14-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']]], DATE 'Nov-18-2022'", "= ARRAY[[[DATE 'Nov-18-2022', DATE 'Oct-15-2022'], [null::date, DATE 'Oct-16-2022']]]" },
        new object[] { "f#-date-null-1array-arraynull", "updateArrayDateIndexFSharp4", "ARRAY[null::date, null::date, null::date, DATE 'Oct-16-2022'], DATE 'Nov-18-2022'", "= ARRAY[DATE 'Nov-18-2022', null::date, null::date, DATE 'Oct-16-2022']" },
        new object[] { "f#-date-null-2array-arraynull", "updateArrayDateIndexFSharp5", "ARRAY[[null::date, null::date], [null::date, DATE 'Oct-16-2022']], DATE 'Nov-18-2022'", "= ARRAY[[DATE 'Nov-18-2022', null::date], [null::date, DATE 'Oct-16-2022']]" },
        new object[] { "f#-date-null-3array-arraynull", "updateArrayDateIndexFSharp6", "ARRAY[[[null::date, null::date], [null::date, DATE 'Oct-16-2022']]], DATE 'Nov-18-2022'", "= ARRAY[[[DATE 'Nov-18-2022', null::date], [null::date, DATE 'Oct-16-2022']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayDateIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
