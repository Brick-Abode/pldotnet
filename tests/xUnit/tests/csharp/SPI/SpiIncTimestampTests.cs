using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncTimestampTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncTimestampTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncTimestamp", Arguments = new List<FunctionArgument> { new FunctionArgument("days", "INTEGER"), new FunctionArgument("hours", "INTEGER"), new FunctionArgument("minutes", "INTEGER") }, ReturnType = "TIMESTAMP", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-timestamp-spi", "SPIIncTimestamp", "2, 6, 25", "= '1989-07-27 18:25:01'::TIMESTAMP" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncTimestamp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncTimestampTestsCSharp : BaseSpiIncTimestampTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    DateTime timestamp = reader.GetFieldValue<DateTime>(reader.GetOrdinal(""TIMESTAMPCOL""));
    timestamp = timestamp.AddDays((double)days);
    timestamp = timestamp.AddHours((double)hours);
    timestamp = timestamp.AddMinutes((double)minutes);
    return timestamp;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}