
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class ModifyFloat8ArrayFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyFloat8ArrayFSharp'
    ";

    public ModifyFloat8ArrayFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyFloat8ArrayFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "FLOAT8[]"), new FunctionArgument("b", "FLOAT8") },
            ReturnType = "FLOAT8[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-float8-1darray-null-dll", "modifyFloat8ArrayFSharpDLL1", "ARRAY[21.0000000000109::FLOAT8, NULL::FLOAT8, 4.521234313421::FLOAT8, 7.412344328978::FLOAT8], 11.0050000000005::FLOAT8", "= ARRAY[11.0050000000005::FLOAT8, NULL::FLOAT8, 4.521234313421::FLOAT8, 7.412344328978::FLOAT8]" },
        new object[] { "f#-float8-2darray-null-dll", "modifyFloat8ArrayFSharpDLL2", "ARRAY[[21.0000000000109::FLOAT8, NULL::FLOAT8], [4.521234313421::FLOAT8, 7.412344328978::FLOAT8]], 11.0050000000005::FLOAT8", "= ARRAY[[11.0050000000005::FLOAT8, NULL::FLOAT8], [4.521234313421::FLOAT8, 7.412344328978::FLOAT8]]" },
        new object[] { "f#-float8-2darray-null-dll", "modifyFloat8ArrayFSharpDLL3", "ARRAY[[NULL::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]], 11.0050000000005::FLOAT8", "= ARRAY[[11.0050000000005::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]]" },
        new object[] { "f#-float8-2darray-null-dll", "modifyFloat8ArrayFSharpDLL4", "ARRAY[[NULL::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]], NULL::FLOAT8", "= ARRAY[[0.0::FLOAT8, NULL::FLOAT8], [NULL::FLOAT8, NULL::FLOAT8]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyFloat8ArrayFsharpDll(string featureName, string testName, string input, string expectedResult)
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
