using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncDoubleTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncDoubleTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncDouble", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "FLOAT8") }, ReturnType = "FLOAT8", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-spi", "SPIIncDouble", "'0.0125215699789'::FLOAT8", "= ' 10.214641792002102'::FLOAT4" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncDouble(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncDoubleTestsCSharp : BaseSpiIncDoubleTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    return (float)(reader.GetFloat(reader.GetOrdinal(""F4COL"")) + a);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}