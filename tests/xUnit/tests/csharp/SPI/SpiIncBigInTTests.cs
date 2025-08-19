using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncBigInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncBigInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncBigInT", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "BIGINT") }, ReturnType = "BIGINT", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int8-spi", "SPIIncBigInt", "214748364700", "= 236223201170" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncBigInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncBigInTTestsCSharp : BaseSpiIncBigInTTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    return (long)(reader.GetInt64(reader.GetOrdinal(""I8COL"")) + a);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}