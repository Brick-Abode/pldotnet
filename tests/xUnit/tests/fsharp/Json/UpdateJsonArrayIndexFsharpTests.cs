
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Json")]
public class UpdateJsonArrayIndexFsharpTests : PlDotNetTest
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

    public UpdateJsonArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateJsonArrayIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "JSON[]"), new FunctionArgument("b", "JSON") },
            ReturnType = "JSON[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
  new object[] 
        { 
            "f#-json-null-1array", 
            "updateJsonArrayIndexFSharp1", 
            "ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]::TEXT" 
        },
        new object[] 
        { 
            "f#-json-null-2array", 
            "updateJsonArrayIndexFSharp2", 
            "ARRAY[['{\"age\": 20, \"name\": \"Mikael\"}'::JSON], ['{\"age\": 25, \"name\": \"Rosicley\"}'::JSON], [null::JSON], ['{\"age\": 30, \"name\": \"Todd\"}'::JSON]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY[['{\"age\": 40, \"name\": \"John Doe\"}'::JSON], ['{\"age\": 25, \"name\": \"Rosicley\"}'::JSON], [null::JSON], ['{\"age\": 30, \"name\": \"Todd\"}'::JSON]]::TEXT" 
        },
        new object[] 
        { 
            "f#-json-null-3array", 
            "updateJsonArrayIndexFSharp3", 
            "ARRAY[[['{\"age\": 20, \"name\": \"Mikael\"}'::JSON], ['{\"age\": 25, \"name\": \"Rosicley\"}'::JSON]], [[null::JSON], ['{\"age\": 30, \"name\": \"Todd\"}'::JSON]]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY[[['{\"age\": 40, \"name\": \"John Doe\"}'::JSON], ['{\"age\": 25, \"name\": \"Rosicley\"}'::JSON]], [[null::JSON], ['{\"age\": 30, \"name\": \"Todd\"}'::JSON]]]::TEXT" 
        },
        new object[] 
        { 
            "f#-json-null-1array-arraynull", 
            "updateJsonArrayIndexFSharp4", 
            "ARRAY[null::JSON, null::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, null::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]::TEXT" 
        },
        new object[] 
        { 
            "f#-json-null-2array-arraynull", 
            "updateJsonArrayIndexFSharp5", 
            "ARRAY[[null::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY[['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]]::TEXT" 
        },
        new object[] 
        { 
            "f#-json-null-3array-arraynull", 
            "updateJsonArrayIndexFSharp6", 
            "ARRAY[[[null::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON", 
            "= ARRAY[[['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]]]::TEXT" 
        },        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateJsonArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
