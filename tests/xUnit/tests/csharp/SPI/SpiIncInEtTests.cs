using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncInEtTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncInEtTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncInEt", Arguments = new List<FunctionArgument> { new FunctionArgument("mask", "INTEGER"), new FunctionArgument("incip", "INTEGER") }, ReturnType = "INET", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-inet-spi", "SPIIncInet", "3 , 20", "= '207.69.188.205/27'::INET" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncInEt(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncInEtTestsCSharp : BaseSpiIncInEtTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    (IPAddress Address, int Netmask) inet = reader.GetFieldValue<(IPAddress Address, int Netmask)>(reader.GetOrdinal(""INETCOL""));
    inet.Netmask += (int)mask;
    byte[] bytes = inet.Address.GetAddressBytes();
    bytes[bytes.Length-1] += (byte)incip;
    inet.Address = new IPAddress(bytes);
    return inet;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}