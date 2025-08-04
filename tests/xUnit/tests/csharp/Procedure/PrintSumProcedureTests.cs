using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BasePrintSumProcedureTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BasePrintSumProcedureTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.Procedure, Name = "printSumProcedure", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-sum", "printSumProcedure1", "10, 25", "= 35" }, new object[] { "c#-sum", "printSumProcedure2", "1450, 275", "= 1725" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSumProcedure(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Procedure")]
public class PrintSumProcedureTestsCSharp : BasePrintSumProcedureTests
{
    protected override string FunctionBody => @"
int c = (int)a + (int)b;
Elog.Info($""c = {c}"");
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}