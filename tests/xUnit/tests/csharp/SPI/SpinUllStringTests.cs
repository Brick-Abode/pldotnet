using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUllStringTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUllStringTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUllString", Arguments = new List<FunctionArgument> { }, ReturnType = "TEXT[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-string-null-spi", "SPINullString", "", "= ARRAY['Hello Terrible World!'::TEXT, NULL::TEXT]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUllString(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUllStringTestsCSharp : BaseSpinUllStringTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<string?> strings = new ();
    while(reader.Read())
    {
        strings.Add(reader.GetFieldValue<string?>(reader.GetOrdinal(""TEXTCOL"")));
    }
    return strings.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}