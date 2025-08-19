using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayMacAddress8IndexFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayMacAddress8IndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayMacAddress8IndexFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "MACADDR8[]"), new FunctionArgument("b", "MACADDR8") }, ReturnType = "MACADDR8[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-macaddr8-1array", "updateArrayMacAddress8IndexFSharp1", "ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']" }, new object[] { "f#-macaddr8-null-1array-arraynull", "updateArrayMacAddress8IndexFSharp2", "ARRAY[null::MACADDR8, null::MACADDR8, null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8, null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']" }, new object[] { "f#-macaddr8-2array", "updateArrayMacAddress8IndexFSharp3", "ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" }, new object[] { "f#-macaddr8-null-2array-arraynull", "updateArrayMacAddress8IndexFSharp4", "ARRAY[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" }, new object[] { "f#-macaddr8-3array", "updateArrayMacAddress8IndexFSharp5", "ARRAY[[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]]" }, new object[] { "f#-macaddr8-null-3array-arraynull", "updateArrayMacAddress8IndexFSharp6", "ARRAY[[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]], MACADDR8 'd1-00-2b-01-02-03-ab-ac'", "= ARRAY[[[MACADDR8 'd1-00-2b-01-02-03-ab-ac', null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayMacAddress8IndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Network")]
public class UpdateArrayMacAddress8IndexFsharpTestsFSharp : BaseUpdateArrayMacAddress8IndexFsharpTests
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