
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class SetNewDatedLlTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!setnewdate'
    ";

    public SetNewDatedLlTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SetNewDatedLl",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_timestamp", "TIMESTAMP"), new FunctionArgument("new_date", "DATE") },
            ReturnType = "TIMESTAMP",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamp-dll", "setNewDateDLL1", "TIMESTAMP '2004-10-19 10:23:54 PM', DATE '2022-10-17'", "= TIMESTAMP '2022-10-17 10:23:54 PM'" },
        new object[] { "c#-timestamp-null-dll", "setNewDateDLL2", "NULL::TIMESTAMP, NULL::DATE", "= TIMESTAMP '2023-12-25 08:30:20'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSetNewDatedLl(string featureName, string testName, string input, string expectedResult)
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
