using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseRecordWithNullTestsPiTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseRecordWithNullTestsPiTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "RecordWithNullTestsPi", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TEXT"), new FunctionArgument("b", "FLOAT8"), new FunctionArgument("c", "MACADDR") }, ReturnType = "BOOL", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-record-spi-null", "RecordWithNullTestSPI3", "'hello world!', '3.14159265358'::FLOAT8, 'f6:30:00:00:00:00'::MACADDR", "IS FALSE" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestRecordWithNullTestsPi(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class RecordWithNullTestsPiTestsCSharp : BaseRecordWithNullTestsPiTests
{
    protected override string FunctionBody => @"
var conn = new NpgsqlConnection();
    conn.Open();
    var command = new NpgsqlCommand($""SELECT (@p1, @p2, @p3)"", conn);
    command.Parameters.AddWithValue(""p1"", NpgsqlDbType.Text, a);
    command.Parameters.AddWithValue(""p2"", NpgsqlDbType.Double, b);
    command.Parameters.AddWithValue(""p3"", NpgsqlDbType.MacAddr, c);
    var reader = command.ExecuteReader();
    reader.Read();
    var record = reader.GetFieldValue<object?[]>(0);
    Elog.Warning(""Record[0] = "" + (string?)record[0]);
    Elog.Warning(""Record[1] = "" + (double?)record[1]);
    Elog.Warning(""Record[2] = "" + (PhysicalAddress?)record[2]);
    return record[0] == null || record[1] == null || record[2] == null;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}