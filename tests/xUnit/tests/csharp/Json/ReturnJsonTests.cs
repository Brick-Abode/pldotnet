
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Json")]
public class ReturnJsonTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if(a==null)
        return ""{\""NULL\"": \""NULL_NULL_NULL\""}"";
    return a;
    ";

    public ReturnJsonTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnJson",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "JSON") },
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
                "returnJson1", 
                "'{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON",
                "= '{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON::TEXT" 
            },
            new object[] 
            { 
                "c#-json", 
                "returnJson2", 
                "'{\"a\":\"Sunday\", \"c\":\"Tuesday\", \"b\":\"Monday\"}'::JSON",
                "= '{\"a\":\"Sunday\", \"c\":\"Tuesday\", \"b\":\"Monday\"}'::JSON::TEXT" 
            },
            new object[] 
            { 
                "c#-json", 
                "returnJson3", 
                "'{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"Tuesday\":\"2022-11-08\"}'::JSON",
                "= '{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"Tuesday\":\"2022-11-08\"}'::JSON::TEXT" 
            },
            new object[] 
            { 
                "c#-json-null", 
                "returnJson4", 
                "null::JSON",
                "= '{\"NULL\": \"NULL_NULL_NULL\"}'::JSON::TEXT" 
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnJson(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
