using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUllDateTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUllDateTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUllDate", Arguments = new List<FunctionArgument> { }, ReturnType = "DATE[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-date-null-spi", "SPINullDate", "", "= ARRAY['2023-02-01'::DATE, NULL::DATE]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUllDate(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUllDateTestsCSharp : BaseSpinUllDateTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<DateOnly?> dates = new ();
    while(reader.Read())
    {
        dates.Add(reader.GetFieldValue<DateOnly?>(reader.GetOrdinal(""DATECOL"")));
    }
    return dates.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}