using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpinUllBooLTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpinUllBooLTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpinUllBooL", Arguments = new List<FunctionArgument> { }, ReturnType = "BOOLEAN[]", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-bool-null-spi", "SPINullBool", "", "= ARRAY[NULL::BOOLEAN, TRUE]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpinUllBooL(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpinUllBooLTestsCSharp : BaseSpinUllBooLTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPINULLS"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    List<bool?> bools = new ();
    while(reader.Read())
    {
        bools.Add(reader.GetFieldValue<bool?>(reader.GetOrdinal(""BOOLCOL"")));
    }
    return bools.ToArray();
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}