using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiteStingCompOudPositionalParametersTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiteStingCompOudPositionalParametersTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiteStingCompOudPositionalParameters", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-multiquery-compoud", "SPITestingCompoudPositionalParameters", "", "= 22" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiteStingCompOudPositionalParameters(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiteStingCompOudPositionalParametersTestsCSharp : BaseSpiteStingCompOudPositionalParametersTests
{
    protected override string FunctionBody => @"
var sb = new StringBuilder();
    sb.Append(""DROP TABLE IF EXISTS SPI_COMPOUD_TESTS;"");
    sb.Append(""CREATE TABLE IF NOT EXISTS SPI_COMPOUD_TESTS (ID INTEGER);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES($1);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES($2);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES($3);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES($4);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES($5);"");
    sb.Append(""UPDATE SPI_COMPOUD_TESTS SET ID = 2 * ID;"");
    sb.Append(""DELETE FROM SPI_COMPOUD_TESTS WHERE ID = $6;"");
    sb.Append(""SELECT * FROM SPI_COMPOUD_TESTS;"");

    using var conn = new NpgsqlConnection();
    conn.Open();
    var cmd = new NpgsqlCommand(sb.ToString(), conn)
    {
        Parameters = { new() { Value = 1 }, new() { Value = 2 }, new() { Value = 3 }, new() { Value = 4 }, new() { Value = 5 }, new() { Value = 8 } }
    };

    var reader = cmd.ExecuteReader();

    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();
    reader.NextResult();

    int sum = 0;
    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        Elog.Info($""Returned value = {_a}"");
        sum += _a;
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}