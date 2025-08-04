using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncInTTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncInTTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncInT", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "INTEGER") }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4-spi", "SPIIncInt", "327670", "= 655340" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncInT(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncInTTestsCSharp : BaseSpiIncInTTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    return (int)(reader.GetInt32(reader.GetOrdinal(""I4COL"")) + a);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}