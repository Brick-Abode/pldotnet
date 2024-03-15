
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class UpdateJsonArrayIndexDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!updatejsonarrayindex'
    ";

    public UpdateJsonArrayIndexDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateJsonArrayIndexDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "JSON[]"), new FunctionArgument("desired", "JSON"), new FunctionArgument("index", "integer[]") },
            ReturnType = "JSON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {"c#-json-null-1array-dll", "updateJsonArrayIndexDLL1", "ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, ARRAY[2]", "= ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]::TEXT"},
            new object[] {"c#-json-null-2array-arraynull-dll", "updateJsonArrayIndexDLL2", "ARRAY[[null::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, ARRAY[1,0]", "= ARRAY[[null::JSON, null::JSON], ['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]]::TEXT"},
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateJsonArrayIndexDll(string featureName, string testName, string input, string expectedResult)
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
