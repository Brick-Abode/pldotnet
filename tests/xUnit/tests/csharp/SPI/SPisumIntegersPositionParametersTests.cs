using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSPisumIntegersPositionParametersTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSPisumIntegersPositionParametersTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SPisumIntegersPositionParameters", Arguments = new List<FunctionArgument> { new FunctionArgument("num", "INTEGER") }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-multiquery", "SPISUMIntegersPositionParameters1", "1", "= 3" }, new object[] { "c#-int-spi-multiquery", "SPISUMIntegersPositionParameters2", "10", "= 30" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSPisumIntegersPositionParameters(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SPisumIntegersPositionParametersTestsCSharp : BaseSPisumIntegersPositionParametersTests
{
    protected override string FunctionBody => @"
var cmd = new NpgsqlCommand($""SELECT $1; SELECT $2"")
    {
        Parameters = { new() { Value = num }, new() { Value = 2*num } }
    };

    var reader = cmd.ExecuteReader();

    int sum = 0;
    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        sum += _a;
    }

    reader.NextResult();

    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        sum += _a;
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}