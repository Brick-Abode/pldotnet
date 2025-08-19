using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncMacAddressTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncMacAddressTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncMacAddress", Arguments = new List<FunctionArgument> { new FunctionArgument("inc", "INTEGER") }, ReturnType = "MACADDR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr-spi", "SPIIncMacAddress", "5", "= 'f6:30:00:00:00:05'::MACADDR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncMacAddress(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncMacAddressTestsCSharp : BaseSpiIncMacAddressTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    PhysicalAddress mac = reader.GetFieldValue<PhysicalAddress>(reader.GetOrdinal(""MACCOL""));
    byte[] bytes = mac.GetAddressBytes();
    bytes[5] += (byte)inc;
    return new PhysicalAddress(bytes);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}