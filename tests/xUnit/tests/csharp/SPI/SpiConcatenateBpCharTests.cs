using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiConcatenateBpCharTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiConcatenateBpCharTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiConcatenateBpChar", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "BPCHAR") }, ReturnType = "BPCHAR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bpchar-spi", "SPIConcatenateBpchar", "'my friend...'::BPCHAR", "= 'good bye  my friend...'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiConcatenateBpChar(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiConcatenateBpCharTestsCSharp : BaseSpiConcatenateBpCharTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    string a = reader.GetFieldValue<string>(reader.GetOrdinal(""CHARCOL""));
    return a + b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}