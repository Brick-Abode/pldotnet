using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BaseSayHelloFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSayHelloFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.Procedure, Name = "sayHelloFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("name", "TEXT") }, ReturnType = "", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-sayHello", "sayHelloFsharp1", "'Mikael'::TEXT", "= Mikael" }, new object[] { "f#-sayHello", "sayHelloFsharp2", "'Rosicley'::TEXT", "= Rosicley" }, new object[] { "f#-sayHello", "sayHelloFsharp3", "'Todd'::TEXT", "= Todd" }, };
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
public class SayHelloFsharpTestsFSharp : BaseSayHelloFsharpTests
{
    protected override string FunctionBody => @"
let message = ""Hello, "" + name + ""! Welcome to plfsharp.""
Elog.Info(message)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}