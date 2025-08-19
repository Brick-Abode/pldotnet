using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayPolygonIndexDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayPolygonIndexDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayPolygonIndexDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "POLYGON[]"), new FunctionArgument("desired", "POLYGON"), new FunctionArgument("index", "integer[]") },
            ReturnType = "POLYGON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
            CastFunctionAs = "text",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] {
                "c#-polygon-null-1array-dll",
                "updateArrayPolygonIndexDLL1",
                "ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[2]",
                "= CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON] AS TEXT)"
            },
            new object[] {
                "c#-polygon-null-2array-arraynull-dll",
                "updateArrayPolygonIndexDLL2",
                "ARRAY[[null::POLYGON, null::POLYGON], [null::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, ARRAY[1,0]",
                "= CAST(ARRAY[[null::POLYGON, null::POLYGON], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::POLYGON]] AS TEXT)"
            },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPolygonIndexDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class UpdateArrayPolygonIndexDllTestsCSharp : BaseUpdateArrayPolygonIndexDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!updatearraypolygonindex'
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