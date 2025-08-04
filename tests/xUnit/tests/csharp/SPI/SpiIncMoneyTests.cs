using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncMoneyTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncMoneyTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncMoney", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "MONEY") }, ReturnType = "MONEY", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-money-spi", "SPIIncMoney", "'1315.23'::MONEY", "= '32731.15'::MONEY" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncMoney(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncMoneyTestsCSharp : BaseSpiIncMoneyTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    decimal money = reader.GetFieldValue<decimal>(reader.GetOrdinal(""MONEYCOL""));
    return money + (decimal)a;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}