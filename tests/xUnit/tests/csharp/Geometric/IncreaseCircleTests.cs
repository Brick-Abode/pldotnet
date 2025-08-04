using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreaseCircleTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreaseCircleTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreaseCircle", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "CIRCLE") }, ReturnType = "CIRCLE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-circle-null", "increaseCircle1", "NULL::CIRCLE", "= CIRCLE '<(0, 0), 4>'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseCircle(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseCircleTestsCSharp : BaseIncreaseCircleTests
{
    protected override string FunctionBody => @"
if (orig_value == null)
    orig_value = new NpgsqlCircle(new NpgsqlPoint(0, 0), 3);

NpgsqlCircle new_value = new NpgsqlCircle(((NpgsqlCircle)orig_value).Center, ((NpgsqlCircle)orig_value).Radius + 1);

return new_value;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}