using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BasePrintSmallestValueProcedureTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BasePrintSmallestValueProcedureTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.Procedure, Name = "findSmallestValueProcedure", Arguments = new List<FunctionArgument> { new FunctionArgument("doublevalues", "double precision[]") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-min", "findSmallestValueProcedure1", "ARRAY[2.25698, 2.85956, 2.85456, 0.00128, 0.00127, 2.36875]", "= 0.00127" }, new object[] { "c#-min", "findSmallestValueProcedure2", "ARRAY[2.25698, -2.85956, 2.85456, -0.00128, 0.00127, 12.36875, -23.2354]", "= -23.2354" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestPrintSmallestValueProcedure(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Procedure")]
public class PrintSmallestValueProcedureTestsCSharp : BasePrintSmallestValueProcedureTests
{
    protected override string FunctionBody => @"
double min = double.MaxValue;
for(int i = 0; i < doublevalues.Length; i++)
{
    double value = (double)doublevalues.GetValue(i);
    min = min < value ? min : value;
}
Elog.Info($""Minimum value = {min}"");
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}