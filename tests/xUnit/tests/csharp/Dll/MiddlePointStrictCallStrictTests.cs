using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMiddlePointStrictCallStrictTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMiddlePointStrictCallStrictTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MiddlePointStrictCallStrict",
            Arguments = new List<FunctionArgument> { new FunctionArgument("pointa", "point"), new FunctionArgument("pointb", "point") },
            ReturnType = "point",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-point-dll", "middlePointStrictCallStrict", "POINT(10.0,20.0), POINT(20.0,40.0)", "~= POINT(15.0,30.0)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMiddlePointStrictCallStrict(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class MiddlePointStrictCallStrictTestsCSharp : BaseMiddlePointStrictCallStrictTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.OtherTests.TestClass!middlePointStrict'
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