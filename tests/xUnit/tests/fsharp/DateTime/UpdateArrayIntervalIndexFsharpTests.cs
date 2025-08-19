using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayIntervalIndexFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayIntervalIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayIntervalIndexFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INTERVAL[]"), new FunctionArgument("b", "INTERVAL") }, ReturnType = "INTERVAL[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-interval-1array", "updateArrayIntervalIndexFSharp1", "ARRAY[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds', null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']" }, new object[] { "f#-interval-2array", "updateArrayIntervalIndexFSharp2", "ARRAY[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]" }, new object[] { "f#-interval-3array", "updateArrayIntervalIndexFSharp3", "ARRAY[[[INTERVAL '4 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', INTERVAL '8 hours 1 minutes 2 seconds'], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]]" }, new object[] { "f#-interval-null-1array-arraynull", "updateArrayIntervalIndexFSharp4", "ARRAY[null::interval, null::interval, null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds'], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval, null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']" }, new object[] { "f#-interval-null-2array-arraynull", "updateArrayIntervalIndexFSharp5", "ARRAY[[null::interval, null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]" }, new object[] { "f#-interval-null-3array-arraynull", "updateArrayIntervalIndexFSharp6", "ARRAY[[[null::interval, null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]], INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds'", "= ARRAY[[[INTERVAL '5 YEAR 4 MONTHS 20 DAYS 10 hours 5 minutes 6 seconds', null::interval], [null::interval, INTERVAL '1 YEAR 8 MONTHS 15 DAYS 10 hours 5 minutes 6 seconds']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayIntervalIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayIntervalIndexFsharpTestsFSharp : BaseUpdateArrayIntervalIndexFsharpTests
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