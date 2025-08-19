using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiModifyLseGTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiModifyLseGTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiModifyLseG", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "POINT") }, ReturnType = "LSEG", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-lseg-spi", "SPIModifyLSeg", "'(3.0,3.0)'::POINT", "= '((1.0,1.0),(3.0,3.0))'::LSEG" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiModifyLseG(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiModifyLseGTestsCSharp : BaseSpiModifyLseGTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlLSeg b = reader.GetFieldValue<NpgsqlLSeg>(reader.GetOrdinal(""LSEGCOL""));
    b.End = (NpgsqlPoint)a;
    return b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}