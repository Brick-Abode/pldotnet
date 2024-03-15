
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class Sum2IntegerDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!sum2integer'
    ";

    public Sum2IntegerDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2IntegerDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer") },
            ReturnType = "integer",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4-dll", "sum2IntegerDLL1", "32770, 100", "= INTEGER '32870'" },
        new object[] { "c#-int4-null-dll", "sum2IntegerDLL2", "NULL::INTEGER, 100::INTEGER", "= INTEGER '100'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2IntegerDll(string featureName, string testName, string input, string expectedResult)
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
