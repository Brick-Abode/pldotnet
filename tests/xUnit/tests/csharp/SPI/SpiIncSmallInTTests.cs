using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncSmallInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncSmallInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncSmallInT", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "SMALLINT") }, ReturnType = "SMALLINT", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int2-spi", "SPIIncSmallInt", "'15'::SMALLINT", "= '2038'::SMALLINT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncSmallInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncSmallInTTestsCSharp : BaseSpiIncSmallInTTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    return (short)(reader.GetInt16(reader.GetOrdinal(""I2COL"")) + a);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}