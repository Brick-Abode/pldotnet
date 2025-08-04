using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncDateTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncDateTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncDate", Arguments = new List<FunctionArgument> { new FunctionArgument("d", "INTEGER"), new FunctionArgument("m", "INTEGER"), new FunctionArgument("y", "INTEGER") }, ReturnType = "DATE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-date-spi", "SPIIncDate", "5,2,10", "= '1999-09-30'::DATE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncDate(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncDateTestsCSharp : BaseSpiIncDateTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    DateOnly date = reader.GetFieldValue<DateOnly>(reader.GetOrdinal(""DATECOL""));
    date = date.AddDays((int)d);
    date = date.AddMonths((int)m);
    date = date.AddYears((int)y);
    return date;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}