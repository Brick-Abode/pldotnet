using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Procedure")]
public class SayHelloFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let message = ""Hello, "" + name + ""! Welcome to plfsharp.""
Elog.Info(message)
    ";

    public SayHelloFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
			TestType = SqlTestType.Procedure,
            Name = "sayHelloFsharp",
            Arguments = new List<FunctionArgument> {
            	new FunctionArgument("name", "TEXT")
        	},
            ReturnType = "",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-sayHello", "sayHelloFsharp1", "'Mikael'::TEXT", "= Mikael" },
            new object[] { "f#-sayHello", "sayHelloFsharp2", "'Rosicley'::TEXT", "= Rosicley" },
            new object[] { "f#-sayHello", "sayHelloFsharp3", "'Todd'::TEXT", "= Todd" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSumProcedureFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
