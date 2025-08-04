using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncIntervalTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncIntervalTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncInterval", Arguments = new List<FunctionArgument> { new FunctionArgument("days", "INTEGER"), new FunctionArgument("months", "INTEGER") }, ReturnType = "INTERVAL", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-interval-spi", "SPIIncInterval", "60, 1", "= '1 month 3589 days 07:18:16'::INTERVAL" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncInterval(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncIntervalTestsCSharp : BaseSpiIncIntervalTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlInterval orig_interval = reader.GetFieldValue<NpgsqlInterval>(reader.GetOrdinal(""INTERVALCOL""));
    NpgsqlInterval new_interval = new NpgsqlInterval(orig_interval.Months + (int)months, orig_interval.Days + (int)days, orig_interval.Time);
    return new_interval;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}