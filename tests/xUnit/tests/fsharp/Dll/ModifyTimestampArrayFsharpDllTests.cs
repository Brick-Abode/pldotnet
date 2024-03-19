
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class ModifyTimestampArrayFsharpDllTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/net6.0/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!modifyTimestampArray'
    ";

    public ModifyTimestampArrayFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyTimestampArrayFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TIMESTAMP[]"), new FunctionArgument("b", "TIMESTAMP") },
            ReturnType = "TIMESTAMP[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-timestamp-1array-null-dll", "modifyTimestampArrayFSharpDLL1", "ARRAY['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP], NULL::TIMESTAMP", "= ARRAY['2022-11-15 13:23:45'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP, NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]" },
        new object[] { "f#-timestamp-2array-null-dll", "modifyTimestampArrayFSharpDLL2", "ARRAY[['2004-12-19 10:23:54 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP", "= ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, '2020-10-19 10:23:54 PM'::TIMESTAMP], [NULL::TIMESTAMP, '2022-12-25 10:23:54 PM'::TIMESTAMP]]" },
        new object[] { "f#-timestamp-2array-null-dll", "modifyTimestampArrayFSharpDLL3", "ARRAY[[NULL::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]], '2023-01-01 12:12:12 PM'::TIMESTAMP", "= ARRAY[['2023-01-01 12:12:12 PM'::TIMESTAMP, NULL::TIMESTAMP], [NULL::TIMESTAMP, NULL::TIMESTAMP]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyTimestampArrayFsharpDll(string featureName, string testName, string input, string expectedResult)
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
