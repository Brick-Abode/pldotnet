using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseStringTwoDimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseStringTwoDimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "StringTwoDimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "TEXT", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-text-spi-array", "StringTwoDimensionalArray", "", "= 'abcdefghijkl abcdefghijkl'::TEXT" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestStringTwoDimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class StringTwoDimensionalArrayTestsCSharp : BaseStringTwoDimensionalArrayTests
{
    protected override string FunctionBody => @"
var conn = new NpgsqlConnection();
    conn.Open();

    var cmd = new NpgsqlCommand(""SELECT @p1"");
    var p1 = new NpgsqlParameter(""p1"", NpgsqlDbType.Array | NpgsqlDbType.Text);
    cmd.Parameters.Add(p1);
    p1.Value = new string[,] {
        {""a"", ""b"", ""c""},
        {""d"", ""e"", ""f""},
        {""g"", ""h"", ""i""},
        {""j"", ""k"", ""l""},
    };

    var reader = cmd.ExecuteReader();
    reader.Read();

    string sum = string.Empty;

    /// Testing GetValue()
    Elog.Info($""Returned type of GetValue: {reader.GetValue(0).GetType()}"");
    string[,] firstArray = (string[,])reader.GetValue(0);
    for (int i = 0; i < firstArray.GetLength(0); i++)
    {
        for (int j = 0; j < firstArray.GetLength(1); j++)
        {
            sum += firstArray[i, j];
        }
    }

    sum += "" "";

    /// Testing GetFieldValue<T>()
    Elog.Info($""Returned type of GetFieldValue<T>: {reader.GetFieldValue<string[,]>(0).GetType()}"");
    string[,] secondArray = reader.GetFieldValue<string[,]>(0);
    for (int i = 0; i < secondArray.GetLength(0); i++)
    {
        for (int j = 0; j < secondArray.GetLength(1); j++)
        {
            sum += secondArray[i, j];
        }
    }
    return sum;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}