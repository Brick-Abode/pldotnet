
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class ModifyFloat4ArrayFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyFloat4ArrayFSharp'
    ";

    public ModifyFloat4ArrayFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyFloat4ArrayFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "FLOAT4[]"), new FunctionArgument("b", "FLOAT4") },
            ReturnType = "FLOAT4[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float4-1darray-null-dll", "modifyFloat4ArrayFSharpDLL1", "ARRAY[1.50055::FLOAT4, NULL::FLOAT4, 4.52123::FLOAT4, 7.41234::FLOAT4], 12.121212::FLOAT4", "= ARRAY[12.121212::FLOAT4, null::FLOAT4, 4.52123::FLOAT4, 7.41234::FLOAT4]" },
        	new object[] { "f#-float4-2darray-null-dll", "modifyFloat4ArrayFSharpDLL2", "ARRAY[[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]], 3.141516::FLOAT4", "= ARRAY[[3.141516::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]]" },
        	new object[] { "f#-float4-2darray-null-dll", "modifyFloat4ArrayFSharpDLL3", "ARRAY[[1.50055::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], 12.121212::FLOAT4", "= ARRAY[[12.121212::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]]" },
        	new object[] { "f#-float4-3darray-null-dll", "modifyFloat4ArrayFSharpDLL4", "ARRAY[[[1.50055::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], [[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]]], 3.141516::FLOAT4", "= ARRAY[[[3.141516::FLOAT4, NULL::FLOAT4], [NULL::FLOAT4, NULL::FLOAT4]], [[1.50055::FLOAT4, NULL::FLOAT4], [4.52123::FLOAT4, 7.41234::FLOAT4]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyFloat4ArrayFsharpDll(string featureName, string testName, string input, string expectedResult)
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
