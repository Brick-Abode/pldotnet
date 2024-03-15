
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class ModifyVarBitDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!modifyvarbit'
    ";

    public ModifyVarBitDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyVarBitDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a BIT", "VARYING") },
            ReturnType = "BIT VARYING",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varbit-dll", "modifyVarbitDLL1", "'1001110001000'::BIT VARYING", "= '0001110001001'::BIT VARYING" },
        new object[] { "c#-varbit-null-dll", "modifyVarbitDLL2", "NULL::BIT VARYING", "IS NULL" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyVarBitDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
    public override string GetFunctionDefinition(SqlFunctionInfo functionInfo)
    {
        var arguments = string.Join(", ", functionInfo.Arguments.Select(arg => $"{arg.Name} {arg.Type}"));
        string strictKeyword = functionInfo.IsStrict ? "STRICT" : "";

        // Conditionally build the returnTypeString
        string returnTypeString = string.IsNullOrEmpty(functionInfo.ReturnType)
                                    ? string.Empty
                                    : $"RETURNS {functionInfo.ReturnType}";

        return $@"CREATE OR REPLACE FUNCTION {functionInfo.Name}({arguments})
{returnTypeString} AS {functionInfo.Body} LANGUAGE {functionInfo.LanguageString} {strictKeyword};";
    }
}
