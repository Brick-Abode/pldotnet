
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class Mult2InTFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!mult2IntFSharp'
    ";

    public Mult2InTFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Mult2InTFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT4"), new FunctionArgument("b", "INT4") },
            ReturnType = "INT4",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int4-dll", "mult2IntFSharpDLL1", "'25'::INT2, '30'::INT2", "= '750'::INT4" },
        new object[] { "f#-int4-null-dll", "mult2IntFSharpDLL2", "'25'::INT2, NULL::INT2", "= '25'::INT4" },
        new object[] { "f#-int4-null-dll", "mult2IntFSharpDLL3", "NULL::INT2, '30'::INT2", "= '30'::INT4" },
        new object[] { "f#-int4-null-dll", "mult2IntFSharpDLL4", "NULL::INT2, NULL::INT2", "IS NULL" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMult2InTFsharpDll(string featureName, string testName, string input, string expectedResult)
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
