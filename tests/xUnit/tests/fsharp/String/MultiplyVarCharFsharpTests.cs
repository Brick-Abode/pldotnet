
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class MultiplyVarCharFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    let mutable  c:string = """"
    let i = 0
    for i in 1..b do
        c <- c + a
    c.ToUpper()
    ";

    public MultiplyVarCharFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "MultiplyVarCharFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "VARCHAR"), new FunctionArgument("b", "int") },
            ReturnType = "VARCHAR",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varchar", "multiplyVarCharFSharp", "'hello '::VARCHAR, 5", "= 'HELLO HELLO HELLO HELLO HELLO '::VARCHAR" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMultiplyVarCharFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
