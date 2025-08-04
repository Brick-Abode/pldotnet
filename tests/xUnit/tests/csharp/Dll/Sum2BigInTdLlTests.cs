using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSum2BigInTdLlTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSum2BigInTdLlTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2BigInTdLl",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "bigint"), new FunctionArgument("b", "bigint") },
            ReturnType = "bigint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8-dll", "sum2BigIntDLL1", "9223372036854775707, 100", "= bigint '9223372036854775807'" }, new object[] { "c#-int8-null-dll", "sum2BigIntDLL2", "9223372036854775707::BIGINT, NULL::BIGINT", "= bigint '9223372036854775707'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2BigInTdLl(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class Sum2BigInTdLlTestsCSharp : BaseSum2BigInTdLlTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!sum2bigint'
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
    public override string GetFunctionDefinition(SqlFunctionInfo functionInfo)
    {
        var arguments = string.Join(", ", functionInfo.Arguments.Select(arg => $"{arg.Name} {arg.Type}"));
        string strictKeyword = functionInfo.IsStrict ? "STRICT" : "";
        // Conditionally build the returnTypeString
        string returnTypeString = string.IsNullOrEmpty(functionInfo.ReturnType) ? string.Empty : $"RETURNS {functionInfo.ReturnType}";
        return $@"CREATE OR REPLACE FUNCTION {functionInfo.Name}({arguments})
{returnTypeString} AS {functionInfo.Body} LANGUAGE {functionInfo.LanguageString} {strictKeyword};";
    }
}