using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseLinesFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseLinesFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseLinesFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LINE[]") }, ReturnType = "LINE[]", Body = FunctionBody, Language = Language, IsStrict = true, CastFunctionAs = "TEXT" };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-line-1array", "IncreaseLinesFSharp1", "ARRAY[LINE '{-4.5,5.75,-7.25}', LINE '{-46.5,32.75,-54.5}', null::LINE, LINE '{-1.5,2.75,-3.25}']", "= CAST(ARRAY[LINE '{-3.5,6.75,-6.25}', LINE '{-45.5,33.75,-53.5}', LINE '{1,1,1}', LINE '{-0.5,3.75,-2.25}'] AS TEXT)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseLinesFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Geometric")]
public class IncreaseLinesFsharpTestsFSharp : BaseIncreaseLinesFsharpTests
{
    protected override string FunctionBody => @"
    let flatten_values = Array.CreateInstance(typeof<NpgsqlLine>, values_array.Length)
    ArrayManipulation.FlatArray(values_array, ref flatten_values) |> ignore
    for i in 0 .. flatten_values.Length - 1 do
        if System.Object.ReferenceEquals(flatten_values.GetValue(i), null) then
            ()
        else
            let orig_value = flatten_values.GetValue(i) :?> NpgsqlLine
            let new_value = NpgsqlLine(orig_value.A + 1., orig_value.B + 1., orig_value.C + 1.)
            flatten_values.SetValue(new_value, i)
    flatten_values
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}