using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyInT8RangeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyInT8RangeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyInT8Range", Arguments = new List<FunctionArgument> { new FunctionArgument("lower", "BOOLEAN"), new FunctionArgument("value", "INT8") }, ReturnType = "INT8RANGE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8range-spi", "SPIModifyInt8Range1", "true, 2147483657", "= '(2147483657,9223372036854775804)'::INT8RANGE" }, new object[] { "c#-int8range-spi", "SPIModifyInt8Range2", "false, -10", "= '[,9223372036854775794)'::INT8RANGE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyInT8Range(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyInT8RangeTestsCSharp : BaseSpiModifyInT8RangeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlRange<long> range = reader.GetFieldValue<NpgsqlRange<long>>(reader.GetOrdinal(""I8RCOL""));
    long lowerBound = (bool)lower ? range.LowerBound + (long)value : range.LowerBound;
    long upperBound = (bool)lower ? range.UpperBound : range.UpperBound + (long)value;
    return new NpgsqlRange<long>(lowerBound, range.LowerBoundIsInclusive, range.LowerBoundInfinite && lowerBound == 0, upperBound, range.UpperBoundIsInclusive, range.UpperBoundInfinite && upperBound == 0);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}