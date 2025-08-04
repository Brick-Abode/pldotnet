using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseAddGoodbyeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseAddGoodbyeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "AddGoodbye", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BPCHAR") }, ReturnType = "BPCHAR", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bpchar", "testingBpChar", "'HELLO!'", "= 'HELLO! Goodbye ^.^'::BPCHAR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddGoodbye(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class AddGoodbyeTestsCSharp : BaseAddGoodbyeTests
{
    protected override string FunctionBody => @"
return (a + "" Goodbye ^.^"");
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}