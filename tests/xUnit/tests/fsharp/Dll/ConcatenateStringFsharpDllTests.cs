using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseConcatenateStringFsharpDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseConcatenateStringFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ConcatenateStringFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TEXT"), new FunctionArgument("b", "TEXT") },
            ReturnType = "TEXT",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-text-dll", "concatenateStringFSharpDLL1", "'Neymar'::TEXT, 'Jr.'::TEXT", "= 'Neymar Jr.'::TEXT" }, new object[] { "f#-text-null-dll", "concatenateStringFSharpDLL2", "'Brasil'::TEXT, NULL::TEXT", "= 'Brasil'::TEXT" }, new object[] { "f#-text-null-dll", "concatenateStringFSharpDLL3", "NULL::TEXT, 'Hello World!'::TEXT", "= 'Hello World!'::TEXT" }, new object[] { "f#-text-null-dll", "concatenateStringFSharpDLL4", "NULL::TEXT, NULL::TEXT", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestConcatenateStringFsharpDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class ConcatenateStringFsharpDllTestsFSharp : BaseConcatenateStringFsharpDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!concatenateString'
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
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