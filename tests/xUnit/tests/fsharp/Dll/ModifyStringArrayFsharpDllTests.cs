
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class ModifyStringArrayFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyStringArray'
    ";

    public ModifyStringArrayFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyStringArrayFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR[]"), new FunctionArgument("b", "TEXT") },
            ReturnType = "VARCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varchar-1array-dll", "modifyStringArrayFSharpDLL1", "ARRAY['Alemanha'::VARCHAR, 'Inglaterra'::VARCHAR, 'Espanha'::VARCHAR], 'Portugal'::TEXT", "= ARRAY['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR, 'Espanha'::VARCHAR]" },
        new object[] { "f#-varchar-2array-null-dll", "modifyStringArrayFSharpDLL2", "ARRAY[['Alemanha'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]], 'Portugal'::TEXT", "= ARRAY[['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]]" },
        new object[] { "f#-varchar-2array-null-dll", "modifyStringArrayFSharpDLL3", "ARRAY[[NULL::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]], 'Portugal'::TEXT", "= ARRAY[['Portugal'::VARCHAR, 'Inglaterra'::VARCHAR], ['Espanha'::VARCHAR, NULL::VARCHAR]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyStringArrayFsharpDll(string featureName, string testName, string input, string expectedResult)
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
