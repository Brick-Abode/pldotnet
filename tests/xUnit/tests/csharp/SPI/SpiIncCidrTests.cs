using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncCidrTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncCidrTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncCidr", Arguments = new List<FunctionArgument> { new FunctionArgument("incip", "INTEGER") }, ReturnType = "CIDR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-cidr-spi", "SPIIncCIDR", "45", "= '207.69.188.230/32'::CIDR" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncCidr(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncCidrTestsCSharp : BaseSpiIncCidrTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    (IPAddress Address, int Netmask) inet = reader.GetFieldValue<(IPAddress Address, int Netmask)>(reader.GetOrdinal(""CIDRCOL""));
    byte[] bytes = inet.Address.GetAddressBytes();
    bytes[bytes.Length-1] += (byte)incip;
    inet.Address = new IPAddress(bytes);
    return inet;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}