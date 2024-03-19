
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Call")]
public class CallFibonacciTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
    if (n <= 0)
    {
        Elog.Info(""Fibonacci number must be greater than 0."");
        return null;
    }
    else if (n <= 2)
    {
        return 1;
    }
    return fibonacci(n-1) + fibonacci(n-2);
    ";

    public CallFibonacciTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "fibonacci",
            Arguments = new List<FunctionArgument> {
                new FunctionArgument("n", "integer")
            },
            ReturnType = "BIGINT",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8", "fibonacci1", "5", "= BIGINT '5'" },
            new object[] { "c#-int8", "fibonacci2", "15", "= BIGINT '610'" },
            new object[] { "c#-int8", "fibonacci3", "20", "= BIGINT '6765'" },
            new object[] { "c#-int8", "fibonacci4", "30", "= BIGINT '832040'" },
       };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCallFibonacci(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

