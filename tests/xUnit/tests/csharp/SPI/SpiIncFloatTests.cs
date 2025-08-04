using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncFloatTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncFloatTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncFloat", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "FLOAT4") }, ReturnType = "FLOAT4", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float4-spi", "SPIIncFloat", "'0.01252'::FLOAT4", "= '10.21464'::FLOAT4" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncFloat(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncFloatTestsCSharp : BaseSpiIncFloatTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    return (float)(reader.GetFloat(reader.GetOrdinal(""F4COL"")) + a);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}