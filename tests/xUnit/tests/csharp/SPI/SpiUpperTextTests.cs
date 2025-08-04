using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiUpperTextTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiUpperTextTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiUpperText", Arguments = new List<FunctionArgument> { }, ReturnType = "TEXT", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text-spi", "SPIUpperText", "", "= 'HELLO'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiUpperText(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiUpperTextTestsCSharp : BaseSpiUpperTextTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    string a = reader.GetFieldValue<string>(reader.GetOrdinal(""TEXTCOL""));
    return a.ToUpper();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}