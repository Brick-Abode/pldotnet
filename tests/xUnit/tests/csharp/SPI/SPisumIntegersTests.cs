using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSPisumIntegersTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSPisumIntegersTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SPisumIntegers", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer"), new FunctionArgument("c", "integer") }, ReturnType = "integer", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4-spi", "SPISumIntegers1", "1, 2, 3", "= 6" }, new object[] { "c#-int4-spi", "SPISumIntegers2", "4, 456, 2456", "= 2916" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSPisumIntegers(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SPisumIntegersTestsCSharp : BaseSPisumIntegersTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT {a} as a, {b} as b, {c} as c"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);

    int sum = 0;
    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        int _b = reader.GetInt32(1);
        int _c = reader.GetInt32(2);
        sum += _a + _b + _c;
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}