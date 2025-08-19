using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BasePrintSumProcedureFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BasePrintSumProcedureFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.Procedure, Name = "printSumProcedureFSharp", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-sum", "printSumProcedureFSharp1", "10, 25", "" }, new object[] { "f#-sum", "printSumProcedureFSharp2", "1450, 275", "" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSumProcedureFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Procedure")]
public class PrintSumProcedureFsharpTestsFSharp : BasePrintSumProcedureFsharpTests
{
    protected override string FunctionBody => @"
let c = a.Value + b.Value
Elog.Info($""[F#] c = "" + c.ToString());
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}