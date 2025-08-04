using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayMacAddressIndexFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayMacAddressIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayMacAddressIndexFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "MACADDR[]"), new FunctionArgument("b", "MACADDR") }, ReturnType = "MACADDR[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-macaddr-1array", "updateArrayMacAddressIndexFSharp1", "ARRAY[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03'], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03', null::macaddr, MACADDR 'a8-00-2b-01-02-03']" }, new object[] { "f#-macaddr-2array", "updateArrayMacAddressIndexFSharp2", "ARRAY[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]" }, new object[] { "f#-macaddr-3array", "updateArrayMacAddressIndexFSharp3", "ARRAY[[[MACADDR '08-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[[[MACADDR 'd1-00-2b-01-02-03', MACADDR '09-00-2b-01-02-03'], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]]" }, new object[] { "f#-macaddr-null-1array-arraynull", "updateArrayMacAddressIndexFSharp4", "ARRAY[null::macaddr, null::macaddr, null::macaddr, MACADDR 'a8-00-2b-01-02-03'], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[MACADDR 'd1-00-2b-01-02-03', null::macaddr, null::macaddr, MACADDR 'a8-00-2b-01-02-03']" }, new object[] { "f#-macaddr-null-2array-arraynull", "updateArrayMacAddressIndexFSharp5", "ARRAY[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[[MACADDR 'd1-00-2b-01-02-03', null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]" }, new object[] { "f#-macaddr-null-3array-arraynull", "updateArrayMacAddressIndexFSharp6", "ARRAY[[[null::macaddr, null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]], MACADDR 'd1-00-2b-01-02-03'", "= ARRAY[[[MACADDR 'd1-00-2b-01-02-03', null::macaddr], [null::macaddr, MACADDR 'a8-00-2b-01-02-03']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayMacAddressIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class UpdateArrayMacAddressIndexFsharpTestsFSharp : BaseUpdateArrayMacAddressIndexFsharpTests
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