using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyBitDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyBitDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyBitDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BIT(10)") },
            ReturnType = "BIT(10)",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bit-dll", "modifyBitDLL1", "'10101'::BIT(10)", "= '0010100001'::BIT(10)" }, new object[] { "c#-bit-null-dll", "modifyBitDLL2", "NULL::BIT(10)", "IS NULL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyBitDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class ModifyBitDllTestsCSharp : BaseModifyBitDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!modifybit'
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
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