using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyTimeTzRangeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyTimeTzRangeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyTimeTzRange", Arguments = new List<FunctionArgument> { new FunctionArgument("lower", "BOOLEAN"), new FunctionArgument("days_to_add", "INT"), new FunctionArgument("minutes_do_add", "INT") }, ReturnType = "TSTZRANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }
    public static object[][] TestCases()
    {
        return new object[][]{
        new object[]{
            "c#-tstzrange-spi",
            "SPIModifyTimeTzRange1",
            "true, 0, 10",
            "= '[\"2013-10-01 07:10:00-03\",\"2013-10-01 07:15:00-03\")'::TSTZRANGE"
        },
        new object[]{
            "c#-tstzrange-spi",
            "SPIModifyTimeTzRange2",
            "false, 15, 40",
            "= '[\"2013-10-01 07:00:00-03\",\"2013-10-16 07:55:00-03\")'::TSTZRANGE"
        }
    };
    }


    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyTimeTzRange(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyTimeTzRangeTestsCSharp : BaseSpiModifyTimeTzRangeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlRange<DateTime> range = reader.GetFieldValue<NpgsqlRange<DateTime>>(reader.GetOrdinal(""TSTZRCOL""));
    DateTime lowerBound = (bool)lower ? range.LowerBound.AddDays((int)days_to_add).AddMinutes((double)minutes_do_add) : range.LowerBound;
    DateTime upperBound = (bool)lower ? range.UpperBound : range.UpperBound.AddDays((int)days_to_add).AddMinutes((double)minutes_do_add);
    return new NpgsqlRange<DateTime>(lowerBound, range.LowerBoundIsInclusive, range.LowerBoundInfinite, upperBound, range.UpperBoundIsInclusive, range.UpperBoundInfinite);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}