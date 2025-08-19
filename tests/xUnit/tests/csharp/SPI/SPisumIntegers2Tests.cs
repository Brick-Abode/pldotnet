using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSPisumIntegers2Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSPisumIntegers2Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SPisumIntegers2", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "integer"), new FunctionArgument("b", "integer"), new FunctionArgument("c", "integer") }, ReturnType = "integer", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-multiquery", "SPISumIntegers2", "1, 2, 3", "= 18" }, new object[] { "c#-int-spi-multiquery", "SPISumIntegers2", "4, 0, 5", "= 27" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSPisumIntegers2(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SPisumIntegers2TestsCSharp : BaseSPisumIntegers2Tests
{
    protected override string FunctionBody => @"
using var conn = new NpgsqlConnection();
    conn.Open();
    var command = new NpgsqlCommand($""SELECT {a} as a, {b} as b, {c} as c; SELECT {2*a} as a, {2*b} as b, {2*c} as c"", conn);
    var reader = command.ExecuteReader();

    int sum = 0;
    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        int _b = reader.GetInt32(1);
        int _c = reader.GetInt32(2);
        Elog.Info($""Returned values of the FIRST query: {_a} | {_b} | {_c}"");
        sum += _a + _b + _c;
    }

    reader.NextResult();

    while (reader.Read())
    {
        int _a = reader.GetInt32(0);
        int _b = reader.GetInt32(1);
        int _c = reader.GetInt32(2);
        Elog.Info($""Returned values of the SECOND query: {_a} | {_b} | {_c}"");
        sum += _a + _b + _c;
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}