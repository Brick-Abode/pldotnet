
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class IncreaseInT8RangesDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!increaseint8ranges'
    ";

    public IncreaseInT8RangesDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseInT8RangesDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "INT8RANGE[]") },
            ReturnType = "INT8RANGE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8range-null-1array-dll", "IncreaseInt8RangesDLL1", "ARRAY['[2,6)'::INT8RANGE, '(,6)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]", "= ARRAY['[3,7)'::INT8RANGE, '(,7)'::INT8RANGE, null::INT8RANGE, '[,)'::INT8RANGE]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseInT8RangesDll(string featureName, string testName, string input, string expectedResult)
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
