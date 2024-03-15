
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class UpdateArrayCidrIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayCidrIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayCidrIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "CIDR[]"), new FunctionArgument("b", "CIDR") },
            ReturnType = "CIDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-cidr-1array", "updateArrayCIDRIndexFSharp1", "ARRAY[CIDR '192.168/24', CIDR '170.168/24', null::cidr, CIDR '142.168/24'], CIDR '192.169/24'", "= ARRAY[CIDR '192.169/24', CIDR '170.168/24', null::cidr, CIDR '142.168/24']" },
        new object[] { "f#-cidr-2array", "updateArrayCIDRIndexFSharp2", "ARRAY[[CIDR '192.168/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24'", "= ARRAY[[CIDR '192.169/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']]" },
        new object[] { "f#-cidr-2array", "updateArrayCIDRIndexFSharp3", "ARRAY[[[CIDR '192.168/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']]], CIDR '192.169/24'", "= ARRAY[[[CIDR '192.169/24', CIDR '170.168/24'], [null::cidr, CIDR '142.168/24']]]" },
        new object[] { "f#-cidr-null-1array-arraynull", "updateArrayCIDRIndexFSharp4", "ARRAY[null::CIDR, null::CIDR, null::cidr, CIDR '142.168/24'], CIDR '192.169/24'", "= ARRAY[CIDR '192.169/24', null::CIDR, null::cidr, CIDR '142.168/24']" },
        new object[] { "f#-cidr-null-2array-arraynull", "updateArrayCIDRIndexFSharp5", "ARRAY[[null::CIDR, null::CIDR], [null::cidr, CIDR '142.168/24']], CIDR '192.169/24'", "= ARRAY[[CIDR '192.169/24', null::CIDR], [null::cidr, CIDR '142.168/24']]" },
        new object[] { "f#-cidr-null-3array-arraynull", "updateArrayCIDRIndexFSharp6", "ARRAY[[[null::CIDR, null::CIDR], [null::cidr, CIDR '142.168/24']]], CIDR '192.169/24'", "= ARRAY[[[CIDR '192.169/24', null::CIDR], [null::cidr, CIDR '142.168/24']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayCidrIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
