
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class ByTeaConversionsDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/net6.0/CSharpTest.dll:TestDLLFunctions.TestClass!byteaconversions'
    ";

    public ByTeaConversionsDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ByTeaConversionsDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BYTEA"), new FunctionArgument("b", "BYTEA") },
            ReturnType = "BYTEA",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bytea-dll", "byteaConversionsDLL1", "'Brick Abode is nice!'::BYTEA, 'Thank you very much...'::BYTEA", "= '\\x427269636b2041626f6465206973206e69636521205468616e6b20796f752076657279206d7563682e2e2e'::BYTEA" },
        new object[] { "c#-bytea-null-dll", "byteaConversionsDLL2", "NULL::BYTEA, 'Thank you very much...'::BYTEA", "= 'Thank you very much...'::BYTEA" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestByTeaConversionsDll(string featureName, string testName, string input, string expectedResult)
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
