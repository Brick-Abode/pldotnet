using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncBoxTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncBoxTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncBox", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "FLOAT8") }, ReturnType = "BOX", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-box-spi", "SPIIncBox", "'0.2575'::FLOAT8", "= '((1.2575,1.2575),(2.2575,2.2575))'::BOX" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncBox(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncBoxTestsCSharp : BaseSpiIncBoxTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlBox b = reader.GetFieldValue<NpgsqlBox>(reader.GetOrdinal(""BOXCOL""));
    return new NpgsqlBox(new NpgsqlPoint(b.UpperRight.X + (double)a, b.UpperRight.Y + (double)a), new NpgsqlPoint(b.LowerLeft.X + (double)a, b.LowerLeft.Y + (double)a));
    return b;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}