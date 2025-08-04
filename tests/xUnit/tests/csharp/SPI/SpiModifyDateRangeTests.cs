using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyDateRangeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyDateRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyDateRange", Arguments = new List<FunctionArgument> { new FunctionArgument("lower", "BOOLEAN"), new FunctionArgument("days", "INTEGER"), new FunctionArgument("months", "INTEGER"), new FunctionArgument("years", "INTEGER") }, ReturnType = "DATERANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-daterange-spi", "SPIModifyDateRange1", "true, 10, 5, 0", "= '[2020-06-11,2021-01-01)'::DATERANGE" }, new object[] { "c#-daterange-spi", "SPIModifyDateRange2", "false, 15, 14, 2", "= '[2020-01-01,2024-03-16)'::DATERANGE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyDateRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyDateRangeTestsCSharp : BaseSpiModifyDateRangeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlRange<DateOnly> range = reader.GetFieldValue<NpgsqlRange<DateOnly>>(reader.GetOrdinal(""DRCOL""));
    DateOnly lowerBound = (bool)lower ? range.LowerBound.AddDays((int)days).AddMonths((int)months).AddYears((int)years) : range.LowerBound;
    DateOnly upperBound = (bool)lower ? range.UpperBound : range.UpperBound.AddDays((int)days).AddMonths((int)months).AddYears((int)years);
    return new NpgsqlRange<DateOnly>(lowerBound, range.LowerBoundIsInclusive, range.LowerBoundInfinite, upperBound, range.UpperBoundIsInclusive, range.UpperBoundInfinite);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}