
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Json")]
public class ModifyJsonTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string new_value = $"", \""{b}\"":\""{c}\""""+""}"";
    return a.Replace(""}"", new_value);
    ";

    public ModifyJsonTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyJson",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "JSON"), new FunctionArgument("b", "TEXT"), new FunctionArgument("c", "TEXT") },
            ReturnType = "JSON",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

public static object[][] TestCases()
{
    return new object[][]
    {
        new object[] 
        { 
            "c#-json", 
            "modifyJson1", 
            "'{\"a\":\"Sunday\", \"b\":\"Monday\"}'::JSON, 'c'::TEXT, 'Tuesday'::TEXT", 
            "= '{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON::TEXT" 
        },
        new object[] 
        { 
            "c#-json-null", 
            "modifyJson2", 
            "'{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\"}'::JSON, null::TEXT, null::TEXT", 
            "= '{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"\":\"\"}'::JSON::TEXT" 
        },
    };
}


    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyJson(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
