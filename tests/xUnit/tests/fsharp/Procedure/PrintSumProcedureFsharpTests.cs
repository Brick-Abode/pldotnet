using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Procedure")]
public class PrintSumProcedureFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let c = a.Value + b.Value
Elog.Info($""[F#] c = "" + c.ToString());
    ";

    public PrintSumProcedureFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
			TestType = SqlTestType.Procedure,
            Name = "printSumProcedureFSharp",
            Arguments = new List<FunctionArgument> {
            	new FunctionArgument("a", "integer"),
            	new FunctionArgument("b", "integer")
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
            new object[] { "f#-sum", "printSumProcedureFSharp1", "10, 25", "" },
            new object[] { "f#-sum", "printSumProcedureFSharp2", "1450, 275", "" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSumProcedureFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
