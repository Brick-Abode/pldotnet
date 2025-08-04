using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseStringArrayWithNullTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseStringArrayWithNullTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "StringArrayWithNull", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text-spi-array-null", "StringArrayWithNull", "", "= 6" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestStringArrayWithNull(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class StringArrayWithNullTestsCSharp : BaseStringArrayWithNullTests
{
    protected override string FunctionBody => @"
var conn = new NpgsqlConnection();
    conn.Open();

    var cmd = new NpgsqlCommand(""SELECT @p1"");
    var p1 = new NpgsqlParameter(""p1"", NpgsqlDbType.Array | NpgsqlDbType.Char);
    cmd.Parameters.Add(p1);
    p1.Value = new string[] {null, null, ""aa"", null, ""bb"", ""cc"", null, ""dd"", null, null};

    var reader = cmd.ExecuteReader();
    reader.Read();

    int sum = 0;

    Elog.Info($""Returned type of GetFieldValue<T>: {reader.GetFieldValue<string[]>(0).GetType()}"");
    string[] array = reader.GetFieldValue<string[]>(0);

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