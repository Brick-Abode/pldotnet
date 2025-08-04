using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUlLint4Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUlLint4Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUlLint4", Arguments = new List<FunctionArgument> { }, ReturnType = "INTEGER[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4-null-spi", "SPINullInt4", "", "= ARRAY[NULL::INTEGER, 2023]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUlLint4(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUlLint4TestsCSharp : BaseSpinUlLint4Tests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<int?> ints = new ();
    while(reader.Read())
    {
        ints.Add(reader.GetFieldValue<int?>(reader.GetOrdinal(""I4COL"")));
    }
    return ints.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}