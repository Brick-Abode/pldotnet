using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncTimestampTzTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncTimestampTzTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncTimestampTz", Arguments = new List<FunctionArgument> { new FunctionArgument("days", "INTEGER"), new FunctionArgument("hours", "INTEGER"), new FunctionArgument("minutes", "INTEGER"), new FunctionArgument("seconds", "INTEGER") }, ReturnType = "TIMESTAMP WITH TIME ZONE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-timestamptz-spi", "SPIIncTimestamptz", "2, 6, 25, 40", "= '1989-07-27 18:25:41 America/New_York'::TIMESTAMP" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncTimestampTz(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncTimestampTzTestsCSharp : BaseSpiIncTimestampTzTests
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
    timestamp = timestamp.AddSeconds((double)seconds);
    return timestamp;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}