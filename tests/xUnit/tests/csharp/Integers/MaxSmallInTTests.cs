using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseMaxSmallInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseMaxSmallInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "MaxSmallInT", Arguments = new List<FunctionArgument> { }, ReturnType = "smallint", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int2", "maxSmallInt", "", "= smallint '32767'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestMaxSmallInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class MaxSmallInTTestsCSharp : BaseMaxSmallInTTests
{
    protected override string FunctionBody => @"
return (short)32767;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}