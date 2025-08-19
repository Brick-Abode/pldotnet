using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIntegerArrayWithNullTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIntegerArrayWithNullTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IntegerArrayWithNull", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-array-null", "IntegerArrayWithNull", "", "= 4" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIntegerArrayWithNull(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class IntegerArrayWithNullTestsCSharp : BaseIntegerArrayWithNullTests
{
    protected override string FunctionBody => @"
var conn = new NpgsqlConnection();
    conn.Open();

    var cmd = new NpgsqlCommand(""SELECT @p1"");
    var p1 = new NpgsqlParameter(""p1"", NpgsqlDbType.Array | NpgsqlDbType.Integer);
    cmd.Parameters.Add(p1);
    p1.Value = new int?[] {1, null, 5, 9, null, 10, null, null};

    var reader = cmd.ExecuteReader();
    reader.Read();

    int sum = 0;

    Elog.Info($""Returned type of GetFieldValue<T>: {reader.GetFieldValue<int?[]>(0).GetType()}"");
    int?[] array = reader.GetFieldValue<int?[]>(0);

    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] == null)
        {
            sum++;
        }
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}