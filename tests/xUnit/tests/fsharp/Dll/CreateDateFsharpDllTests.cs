using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateDateFsharpDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateDateFsharpDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateDateFsharpDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INT4"), new FunctionArgument("b", "INT4"), new FunctionArgument("c", "INT4") },
            ReturnType = "DATE",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-date-dll", "createDateFSharpDLL1", "1997, 4, 30", "= 'Apr-30-1997'::DATE" }, new object[] { "f#-date-dll", "createDateFSharpDLL2", "2023, 1, 1", "= 'Jan-01-2023'::DATE" }, new object[] { "f#-date-null-dll", "createDateFSharpDLL3", "2023, 1, NULL", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateFsharpDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Dll")]
public class CreateDateFsharpDllTestsFSharp : BaseCreateDateFsharpDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/fsharp/DotNetTestProject/bin/Release/FSharpTest.dll:TestFSharpDLLFunctions.TestFSharpClass!createDateFSharp'
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
    public override string GetFunctionDefinition(SqlFunctionInfo functionInfo)
    {
        var arguments = string.Join(", ", functionInfo.Arguments.Select(arg => $"{arg.Name} {arg.Type}"));
        string strictKeyword = functionInfo.IsStrict ? "STRICT" : "";
        // Conditionally build the returnTypeString
        string returnTypeString = string.IsNullOrEmpty(functionInfo.ReturnType) ? string.Empty : $"RETURNS {functionInfo.ReturnType}";
        return $@"CREATE OR REPLACE FUNCTION {functionInfo.Name}({arguments})
{returnTypeString} AS {functionInfo.Body} LANGUAGE {functionInfo.LanguageString} {strictKeyword};";
    }
}