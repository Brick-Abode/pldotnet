
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class IncreaseDateOnlyRangesDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increasedateonlyranges'
    ";

    public IncreaseDateOnlyRangesDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseDateOnlyRangesDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "DATERANGE[]") },
            ReturnType = "DATERANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-daterange-null-1array-dll", "IncreaseDateonlyRangesDLL1", "ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-04)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]", "= ARRAY['[2021-01-02, 2021-01-02)'::DATERANGE, '(, 2021-04-05)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseDateOnlyRangesDll(string featureName, string testName, string input, string expectedResult)
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
