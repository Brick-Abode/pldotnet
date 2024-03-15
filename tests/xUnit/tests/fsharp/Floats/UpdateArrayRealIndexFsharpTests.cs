
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Floats")]
public class UpdateArrayRealIndexFsharpTests : PlDotNetTest
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

    public UpdateArrayRealIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayRealIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "real[]"), new FunctionArgument("b", "real") },
            ReturnType = "real[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4-null-1array", "updateArrayRealIndexFSharp1", "ARRAY[4.55555::real, 10.11324::real, null::real], 9.83212", "= ARRAY[9.83212::real, 10.11324::real, null::real]" },
        new object[] { "f#-float4-null-2array", "updateArrayRealIndexFSharp2", "ARRAY[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]], 9.83212", "= ARRAY[[9.83212::real, 10.11324::real], [null::real, 16.12464::real]]" },
        new object[] { "f#-float4-null-3array", "updateArrayRealIndexFSharp3", "ARRAY[[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]]], 9.83212", "= ARRAY[[[9.83212::real, 10.11324::real], [null::real, 16.12464::real]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayRealIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
