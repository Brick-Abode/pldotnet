using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseReturnJsonFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseReturnJsonFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ReturnJsonFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "JSON") }, ReturnType = "JSON", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-json", "returnJsonFSharp1", "'{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON", "= '{\"a\":\"Sunday\", \"b\":\"Monday\", \"c\":\"Tuesday\"}'::JSON::TEXT" }, new object[] { "f#-json", "returnJsonFSharp2", "'{\"a\":\"Sunday\", \"c\":\"Tuesday\", \"b\":\"Monday\"}'::JSON", "= '{\"a\":\"Sunday\", \"c\":\"Tuesday\", \"b\":\"Monday\"}'::JSON::TEXT" }, new object[] { "f#-json", "returnJsonFSharp3", "'{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"Tuesday\":\"2022-11-08\"}'::JSON", "= '{\"Sunday\":\"2022-11-06\", \"Monday\":\"2022-11-07\", \"Tuesday\":\"2022-11-08\"}'::JSON::TEXT" }, new object[] { "f#-json-null", "returnJsonFSharp4", "null::JSON", "= '{\"NULL\": \"NULL_NULL_NULL\"}'::JSON::TEXT" } };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnJsonFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Json")]
public class ReturnJsonFsharpTestsFSharp : BaseReturnJsonFsharpTests
{
    protected override string FunctionBody => @"
if System.Object.ReferenceEquals(a, null) then ""{\""NULL\"": \""NULL_NULL_NULL\""}"" else a
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}