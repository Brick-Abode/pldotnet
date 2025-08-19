using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiConcatenateVarCharTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiConcatenateVarCharTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiConcatenateVarChar", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "VARCHAR") }, ReturnType = "VARCHAR", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-varchar-spi", "SPIConcatenateVarchar", "'my friend...'::VARCHAR", "= 'BYE BYE MY FRIEND...'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiConcatenateVarChar(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiConcatenateVarCharTestsCSharp : BaseSpiConcatenateVarCharTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    string a = reader.GetFieldValue<string>(reader.GetOrdinal(""VARCHARCOL""));
    return (a + "" "" + b).ToUpper();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}