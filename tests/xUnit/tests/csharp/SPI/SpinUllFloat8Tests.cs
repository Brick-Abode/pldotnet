using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUllFloat8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUllFloat8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUllFloat8", Arguments = new List<FunctionArgument> { }, ReturnType = "FLOAT8[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-null-spi", "SPINullFloat8", "", "= ARRAY[NULL::FLOAT8, '3.141592653'::FLOAT8]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUllFloat8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUllFloat8TestsCSharp : BaseSpinUllFloat8Tests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<double?> doubles = new ();
    while(reader.Read())
    {
        doubles.Add(reader.GetFieldValue<double?>(reader.GetOrdinal(""F8COL"")));
    }
    return doubles.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}