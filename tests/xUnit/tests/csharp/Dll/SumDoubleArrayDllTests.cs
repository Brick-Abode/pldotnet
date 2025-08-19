using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSumDoubleArrayDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSumDoubleArrayDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "SumDoubleArrayDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("doubles double", "precision[]") },
            ReturnType = "double precision",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-null-1array-dll", "sumDoubleArrayDLL1", "ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]", "= '32.9335786424099'" }, new object[] { "c#-float8-null-2array-dll", "sumDoubleArrayDLL2", "ARRAY[[21.0000000000109::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]", "= '32.9335786424099'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSumDoubleArrayDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class SumDoubleArrayDllTestsCSharp : BaseSumDoubleArrayDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!sumdoublearray'
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