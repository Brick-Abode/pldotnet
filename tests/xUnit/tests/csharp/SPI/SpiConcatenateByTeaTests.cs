using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiConcatenateByTeaTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiConcatenateByTeaTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiConcatenateByTea", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "BYTEA") }, ReturnType = "BYTEA", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bytea-spi", "SPIConcatenateBytea", "'You are welcome!'::BYTEA", "= 'Thank you! You are welcome!'::BYTEA" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiConcatenateByTea(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiConcatenateByTeaTestsCSharp : BaseSpiConcatenateByTeaTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    byte[] a = reader.GetFieldValue<byte[]>(reader.GetOrdinal(""BYTEACOL""));
    UTF8Encoding utf8_e = new UTF8Encoding();
    string s1 = utf8_e.GetString(a, 0, a.Length);
    string s2 = utf8_e.GetString(b, 0, b.Length);
    Elog.Info($""s1 = {s1} | s2 = {s2}"");
    string result = s1 + "" "" + s2;
    return utf8_e.GetBytes(result);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}