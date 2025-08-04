using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUllMac8Tests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUllMac8Tests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUllMac8", Arguments = new List<FunctionArgument> { }, ReturnType = "MACADDR8[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8-null-spi", "SPINullMac8", "", "= ARRAY['ab:01:2b:31:41:fa:ab:ac'::MACADDR8, NULL::MACADDR8]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUllMac8(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUllMac8TestsCSharp : BaseSpinUllMac8Tests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<PhysicalAddress?> addresses = new ();
    while(reader.Read())
    {
        addresses.Add(reader.GetFieldValue<PhysicalAddress?>(reader.GetOrdinal(""MAC8COL"")));
    }
    return addresses.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}