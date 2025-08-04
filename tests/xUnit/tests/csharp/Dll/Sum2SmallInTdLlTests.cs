using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSum2SmallInTdLlTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSum2SmallInTdLlTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2SmallInTdLl",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "smallint"), new FunctionArgument("b", "smallint") },
            ReturnType = "smallint",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int2-dll", "sum2SmallIntDLL1", "CAST(100 AS smallint), CAST(101 AS smallint)", "= smallint '201'" }, new object[] { "c#-int2-null-dll", "sum2SmallIntDLL2", "NULL::SMALLINT, 30::SMALLINT", "= smallint '30'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2SmallInTdLl(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class Sum2SmallInTdLlTestsCSharp : BaseSum2SmallInTdLlTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!sum2smallint'
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