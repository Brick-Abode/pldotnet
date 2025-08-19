using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseBooleanAndDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseBooleanAndDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "BooleanAndDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "boolean"), new FunctionArgument("b", "boolean") },
            ReturnType = "boolean",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bool-dll", "BooleanAndDLL1", "true, true", "is true" }, new object[] { "c#-bool-null-dll", "BooleanAndDLL2", "NULL::BOOLEAN, true", "is false" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestBooleanAndDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class BooleanAndDllTestsCSharp : BaseBooleanAndDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!booleanand'
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