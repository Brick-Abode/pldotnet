using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncCircleTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncCircleTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncCircle", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "POINT"), new FunctionArgument("b", "float8") }, ReturnType = "CIRCLE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-circle-spi", "SPIIncCircle", "'(1.5,2.5)'::POINT, '1.25354555'::FLOAT8", "= '<(2.5,3.5),1.75354555>'::CIRCLE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncCircle(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncCircleTestsCSharp : BaseSpiIncCircleTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlCircle c = reader.GetFieldValue<NpgsqlCircle>(reader.GetOrdinal(""CIRCLECOL""));
    c.X += ((NpgsqlPoint)a).X;
    c.Y += ((NpgsqlPoint)a).Y;
    c.Radius += (double)b;
    return c;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}