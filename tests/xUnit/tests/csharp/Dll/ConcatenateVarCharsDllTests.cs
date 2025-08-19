using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateVarCharsDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateVarCharsDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateVarCharsDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "VARCHAR"), new FunctionArgument("c", "BPCHAR") },
            ReturnType = "VARCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varchar-dll", "concatenateVarCharsDLL1", "'hello'::VARCHAR, 'beautiful'::VARCHAR, 'world!'::BPCHAR", "= 'HELLO BEAUTIFUL WORLD!'::VARCHAR" }, new object[] { "c#-varchar-null-dll", "concatenateVarCharsDLL2", "NULL::VARCHAR, 'beautiful'::VARCHAR, NULL::BPCHAR", "= ' BEAUTIFUL '::VARCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateVarCharsDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class ConcatenateVarCharsDllTestsCSharp : BaseConcatenateVarCharsDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!concatenatevarchars'
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