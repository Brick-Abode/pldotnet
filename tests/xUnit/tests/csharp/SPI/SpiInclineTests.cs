using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiInclineTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiInclineTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncline", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "LINE") }, ReturnType = "LINE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-line-spi", "SPIIncLine", "'{3.0,2.0,1.0}'::LINE", "= '{4.0,4.0,4.0}'::LINE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncline(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiInclineTestsCSharp : BaseSpiInclineTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlLine b = reader.GetFieldValue<NpgsqlLine>(reader.GetOrdinal(""LINECOL""));
    b.A += ((NpgsqlLine)a).A;
    b.B += ((NpgsqlLine)a).B;
    b.C += ((NpgsqlLine)a).C;
    return b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}