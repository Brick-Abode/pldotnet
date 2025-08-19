using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseMacAddress8DllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseMacAddress8DllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseMacAddress8Dll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR8[]") },
            ReturnType = "MACADDR8[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8-1array-dll", "IncreaseMacAddress8DLL1", "ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a8-00-2b-01-02-03-ab-ac']", "= ARRAY[MACADDR8 '09-00-2b-01-02-03-ab-ac', MACADDR8 '0a-00-2b-01-02-03-ab-ac', null::macaddr, MACADDR8 'a9-00-2b-01-02-03-ab-ac']" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseMacAddress8Dll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class IncreaseMacAddress8DllTestsCSharp : BaseIncreaseMacAddress8DllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!increasemacaddress8'
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