using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIncreasePathTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIncreasePathTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IncreasePath", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "PATH") }, ReturnType = "PATH", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-path-null", "increasePath1", "NULL::PATH", "= '((1,1),(101,101),(201,201))'::PATH" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreasePath(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreasePathTestsCSharp : BaseIncreasePathTests
{
    protected override string FunctionBody => @"
if (orig_value == null)
    orig_value = new NpgsqlPath(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100), new NpgsqlPoint(200, 200));

NpgsqlPath new_value = new NpgsqlPath(((NpgsqlPath)orig_value).Count);
foreach (NpgsqlPoint polygon_point in ((NpgsqlPath)orig_value)) {
    new_value.Add(new NpgsqlPoint(((NpgsqlPoint)polygon_point).X + 1, ((NpgsqlPoint)polygon_point).Y + 1));
}

return new_value;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}