using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyInT4RangeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyInT4RangeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyInT4Range", Arguments = new List<FunctionArgument> { new FunctionArgument("lower", "BOOLEAN"), new FunctionArgument("value", "INT4") }, ReturnType = "INT4RANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4range-spi", "SPIModifyInt4Range1", "true, 48", "= '(-2147483600,2147483644)'::INT4RANGE" }, new object[] { "c#-int4range-spi", "SPIModifyInt4Range2", "false, -10", "= '(-2147483648,2147483634)'::INT4RANGE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyInT4Range(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyInT4RangeTestsCSharp : BaseSpiModifyInT4RangeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlRange<int> range = reader.GetFieldValue<NpgsqlRange<int>>(reader.GetOrdinal(""I4RCOL""));
    int lowerBound = (bool)lower ? range.LowerBound + (int)value : range.LowerBound;
    int upperBound = (bool)lower ? range.UpperBound : range.UpperBound + (int)value;
    return new NpgsqlRange<int>(lowerBound, range.LowerBoundIsInclusive, range.LowerBoundInfinite, upperBound, range.UpperBoundIsInclusive, range.UpperBoundInfinite);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}