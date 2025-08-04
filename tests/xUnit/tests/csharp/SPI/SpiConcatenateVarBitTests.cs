using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiConcatenateVarBitTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiConcatenateVarBitTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiConcatenateVarBit", Arguments = new List<FunctionArgument> { new FunctionArgument("b BIT", "VARYING") }, ReturnType = "BIT VARYING", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varbit-spi", "SPIConcatenateVarBit", "'111010111101111000'::BIT VARYING", "= '100111001111010111101111000'::BIT VARYING" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiConcatenateVarBit(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiConcatenateVarBitTestsCSharp : BaseSpiConcatenateVarBitTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    BitArray a = reader.GetFieldValue<BitArray>(reader.GetOrdinal(""VARBITCOL""));
    BitArray c = new BitArray(a.Length+b.Length);
    for(int i = 0; i < a.Length;i++)
        c[i] = a[i];
    for(int i = 0, cont = a.Length; i < b.Length;i++)
        c[cont++] = b[i];
    return c;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}