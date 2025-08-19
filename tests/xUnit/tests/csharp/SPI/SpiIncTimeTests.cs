using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncTimeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncTimeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncTime", Arguments = new List<FunctionArgument> { new FunctionArgument("m", "INTEGER"), new FunctionArgument("h", "INTEGER") }, ReturnType = "TIME", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-time-spi", "SPIIncTime", "24, 2", "= '14:24:01'::TIME" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncTime(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncTimeTestsCSharp : BaseSpiIncTimeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    TimeOnly time = reader.GetFieldValue<TimeOnly>(reader.GetOrdinal(""TIMECOL""));
    time = time.AddMinutes((double)m);
    time = time.AddHours((double)h);
    return time;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}