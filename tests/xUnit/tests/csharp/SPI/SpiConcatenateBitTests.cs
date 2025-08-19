using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiConcatenateBitTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiConcatenateBitTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiConcatenateBit", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "BIT(10)") }, ReturnType = "BIT", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bit-spi", "SPIConcatenateBit", "'1110101'::BIT(10)", "= '100110011110101000'::BIT(18)" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiConcatenateBit(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiConcatenateBitTestsCSharp : BaseSpiConcatenateBitTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    BitArray a = reader.GetFieldValue<BitArray>(reader.GetOrdinal(""BITCOL""));
    BitArray c = new BitArray(a.Length+b.Length);
    for(int i = 0; i < a.Length;i++)
        c[i] = a[i];
    for(int i = 0, cont = a.Length; i < b.Length;i++)
        c[cont++] = b[i];
    return c;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}