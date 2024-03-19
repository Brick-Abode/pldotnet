
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class CombineUUIdsDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!combineuuids'
    ";

    public CombineUUIdsDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CombineUUIdsDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID"), new FunctionArgument("b", "UUID") },
            ReturnType = "UUID",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-uuid-dll", "combineUUIDsDLL1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID", "= 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID" },
        new object[] { "c#-uuid-dll", "combineUUIDsDLL2", "'123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= '123e4567-e89b-12d3-9694-12769239b763'::UUID" },
        new object[] { "c#-uuid-null-dll", "combineUUIDsDLL3", "NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCombineUUIdsDll(string featureName, string testName, string input, string expectedResult)
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
