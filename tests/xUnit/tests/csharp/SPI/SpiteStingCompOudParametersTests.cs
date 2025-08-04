using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiteStingCompOudParametersTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiteStingCompOudParametersTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiteStingCompOudParameters", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-multiquery-compoud", "SPITestingCompoudParameters", "", "= 22" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiteStingCompOudParameters(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiteStingCompOudParametersTestsCSharp : BaseSpiteStingCompOudParametersTests
{
    protected override string FunctionBody => @"
var sb = new StringBuilder();
    sb.Append(""DROP TABLE IF EXISTS SPI_COMPOUD_TESTS;"");
    sb.Append(""CREATE TABLE IF NOT EXISTS SPI_COMPOUD_TESTS (ID INTEGER);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@a);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@b);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@c);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@d);"");
    sb.Append(""INSERT INTO SPI_COMPOUD_TESTS (ID) VALUES(@e);"");
    sb.Append(""UPDATE SPI_COMPOUD_TESTS SET ID = 2 * ID;"");
    sb.Append(""DELETE FROM SPI_COMPOUD_TESTS WHERE ID = @f;"");
    sb.Append(""SELECT * FROM SPI_COMPOUD_TESTS;"");

    using var conn = new NpgsqlConnection();
    conn.Open();
    var cmd = new NpgsqlCommand(sb.ToString(), conn);

    cmd.Parameters.AddWithValue(""a"", NpgsqlDbType.Integer, 1);
    cmd.Parameters.AddWithValue(""b"", NpgsqlDbType.Integer, 2);
    cmd.Parameters.AddWithValue(""c"", NpgsqlDbType.Integer, 3);
    cmd.Parameters.AddWithValue(""d"", NpgsqlDbType.Integer, 4);
    cmd.Parameters.AddWithValue(""e"", NpgsqlDbType.Integer, 5);

    cmd.Parameters.AddWithValue(""f"", NpgsqlDbType.Integer, 8);

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