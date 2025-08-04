using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncTimeWithTimeZonZeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncTimeWithTimeZonZeTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncTimeWithTimeZonZe", Arguments = new List<FunctionArgument> { new FunctionArgument("hours", "FLOAT4") }, ReturnType = "TIME WITH TIME ZONE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-timetz-spi", "SPIIncTimeWithTimeZonze", "1.75", "= '07:15-03:00'::TIMETZ" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncTimeWithTimeZonZe(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncTimeWithTimeZonZeTestsCSharp : BaseSpiIncTimeWithTimeZonZeTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    DateTimeOffset timetz = reader.GetFieldValue<DateTimeOffset>(reader.GetOrdinal(""TIMETZCOL""));
    return timetz.AddHours((double)hours);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}