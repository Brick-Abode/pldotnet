
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class Sum2SmallInTFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!sum2SmallIntFSharp'
    ";

    public Sum2SmallInTFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2SmallInTFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT2"), new FunctionArgument("b", "INT2") },
            ReturnType = "INT2",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int2-dll", "sum2SmallIntFSharpDLL1", "'25'::INT2, '250'::INT2", "= '275'::INT2" },
			new object[] { "f#-int2-dll", "sum2SmallIntFSharpDLL2", "'1997'::INT2, '25'::INT2", "= '2022'::INT2" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2SmallInTFsharpDll(string featureName, string testName, string input, string expectedResult)
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
