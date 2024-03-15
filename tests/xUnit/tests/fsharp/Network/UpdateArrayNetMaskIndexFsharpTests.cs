
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class UpdateArrayNetMaskIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayNetMaskIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayNetMaskIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INET[]"), new FunctionArgument("b", "INET") },
            ReturnType = "INET[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-inet-1array", "updateArrayNetMaskIndexFSharp1", "ARRAY[INET '192.168.0.1/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24'], INET '192.168.0.120/24'", "= ARRAY[INET '192.168.0.120/24', INET '192.170.0.1/24', null::inet, INET '170.168.0.1/24']" },
        new object[] { "f#-inet-2array", "updateArrayNetMaskIndexFSharp2", "ARRAY[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24'", "= ARRAY[[INET '192.168.0.120/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']]" },
        new object[] { "f#-inet-2array", "updateArrayNetMaskIndexFSharp3", "ARRAY[[[INET '192.168.0.1/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']]], INET '192.168.0.120/24'", "= ARRAY[[[INET '192.168.0.120/24', INET '192.170.0.1/24'], [null::inet, INET '170.168.0.1/24']]]" },
        new object[] { "f#-inet-null-1array-arraynull", "updateArrayNetMaskIndexFSharp4", "ARRAY[null::INET, null::INET, null::inet, INET '170.168.0.1/24'], INET '192.168.0.120/24'", "= ARRAY[INET '192.168.0.120/24', null::INET, null::inet, INET '170.168.0.1/24']" },
        new object[] { "f#-inet-null-2array-arraynull", "updateArrayNetMaskIndexFSharp5", "ARRAY[[null::INET, null::INET], [null::inet, INET '170.168.0.1/24']], INET '192.168.0.120/24'", "= ARRAY[[INET '192.168.0.120/24', null::INET], [null::inet, INET '170.168.0.1/24']]" },
        new object[] { "f#-inet-null-3array-arraynull", "updateArrayNetMaskIndexFSharp6", "ARRAY[[[null::INET, null::INET], [null::inet, INET '170.168.0.1/24']]], INET '192.168.0.120/24'", "= ARRAY[[[INET '192.168.0.120/24', null::INET], [null::inet, INET '170.168.0.1/24']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayNetMaskIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
