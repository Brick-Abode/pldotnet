using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyTimeRangeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyTimeRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyTimeRange", Arguments = new List<FunctionArgument> { new FunctionArgument("lower", "BOOLEAN"), new FunctionArgument("days_to_add", "INT"), new FunctionArgument("minutes_do_add", "INT") }, ReturnType = "TSRANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-tsrange-spi", "SPIModifyTimeRange1", "true, 0, 15", "= '[2010-01-01 14:45, 2010-01-01 15:30)'::TSRANGE" }, new object[] { "c#-tsrange-spi", "SPIModifyTimeRange2", "false, 10, 25", "= '[2010-01-01 14:30, 2010-01-11 15:55)'::TSRANGE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyTimeRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyTimeRangeTestsCSharp : BaseSpiModifyTimeRangeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlRange<DateTime> range = reader.GetFieldValue<NpgsqlRange<DateTime>>(reader.GetOrdinal(""TSRCOL""));
    DateTime lowerBound = (bool)lower ? range.LowerBound.AddDays((int)days_to_add).AddMinutes((double)minutes_do_add) : range.LowerBound;
    DateTime upperBound = (bool)lower ? range.UpperBound : range.UpperBound.AddDays((int)days_to_add).AddMinutes((double)minutes_do_add);
    return new NpgsqlRange<DateTime>(lowerBound, range.LowerBoundIsInclusive, range.LowerBoundInfinite, upperBound, range.UpperBoundIsInclusive, range.UpperBoundInfinite);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}