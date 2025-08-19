using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncMacAddress8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncMacAddress8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncMacAddress8", Arguments = new List<FunctionArgument> { new FunctionArgument("inc", "INTEGER") }, ReturnType = "MACADDR8", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8-spi", "SPIIncMacAddress8", "3", "= 'f6:30:00:ff:fe:00:00:03'::MACADDR8" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncMacAddress8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncMacAddress8TestsCSharp : BaseSpiIncMacAddress8Tests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    PhysicalAddress mac = reader.GetFieldValue<PhysicalAddress>(reader.GetOrdinal(""MAC8COL""));
    byte[] bytes = mac.GetAddressBytes();
    bytes[7] += (byte)inc;
    return new PhysicalAddress(bytes);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}