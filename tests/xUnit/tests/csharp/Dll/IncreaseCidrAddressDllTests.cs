using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseCidrAddressDllTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseCidrAddressDllTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseCidrAddressDll",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "CIDR[]") },
            ReturnType = "CIDR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-cidr-1array-dll", "IncreaseCIDRAddressDLL1", "ARRAY[CIDR '192.168.231.0/24', CIDR '175.170.14.0/24', null::cidr, CIDR '167.168.41.0/24']", "= ARRAY[CIDR '193.168.231.0/24', CIDR '176.170.14.0/24', null::cidr, CIDR '168.168.41.0/24']" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseCidrAddressDll(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Dll")]
public class IncreaseCidrAddressDllTestsCSharp : BaseIncreaseCidrAddressDllTests
{
    protected override string FunctionBody => @"
'/app/pldotnet/tests/csharp/DotNetTestProject/bin/Release/CSharpTest.dll:TestDLLFunctions.TestClass!increasecidraddress'
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