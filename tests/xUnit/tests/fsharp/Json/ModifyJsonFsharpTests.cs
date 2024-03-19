
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Json")]
public class ModifyJsonFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let new_value = String.Concat("", \"""", b, ""\"":\"""", c, ""\""}"")
    a.Replace(""}"", new_value)
    ";

    public ModifyJsonFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyJsonFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "JSON"), new FunctionArgument("b", "TEXT"), new FunctionArgument("c", "TEXT") },
            ReturnType = "JSON",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
   new object[] 
            { 
                "f#-json", 
                "modifyJsonFSharp1", 
                 "'{\"a\":\"Sunday\", \"b\":\"Monday\"}'::JSON, 'c'::TEXT, 'Tuesday'::TEXT", 
            "= '{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON::TEXT" 
            },
        new object[] 
            { 
                "f#-json-null", 
                "modifyJsonFSharp2", 
                 "'{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\"}'::JSON, null::TEXT, null::TEXT", 
            "= '{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"\":\"\"}'::JSON::TEXT" 
            }          };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyJsonFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
