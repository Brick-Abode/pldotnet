using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseIntegerOneDimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseIntegerOneDimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "IntegerOneDimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int-spi-array", "IntegerOneDimensionalArray", "", "= 30" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIntegerOneDimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class IntegerOneDimensionalArrayTestsCSharp : BaseIntegerOneDimensionalArrayTests
{
    protected override string FunctionBody => @"
var conn = new NpgsqlConnection();
    conn.Open();

    var cmd = new NpgsqlCommand(""SELECT @p1"");
    var p1 = new NpgsqlParameter(""p1"", NpgsqlDbType.Array | NpgsqlDbType.Integer);
    cmd.Parameters.Add(p1);
    p1.Value = new int[] {1, 5, 9};

    var reader = cmd.ExecuteReader();
    reader.Read();

    int sum = 0;

    /// Testing GetValue()
    Elog.Info($""Returned type of GetValue: {reader.GetValue(0).GetType()}"");
    int[] firstArray = (int[])reader.GetValue(0);
    for (int i = 0; i < firstArray.Length; i++)
    {
        sum += firstArray[i];
    }

    /// Testing GetFieldValue<T>()
    Elog.Info($""Returned type of GetFieldValue<T>: {reader.GetFieldValue<int[]>(0).GetType()}"");
    int[] secondArray = reader.GetFieldValue<int[]>(0);
    for (int i = 0; i < secondArray.Length; i++)
    {
        sum += secondArray[i];
    }

    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}