using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiCombineUUIdTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiCombineUUIdTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiCombineUUId", Arguments = new List<FunctionArgument> { new FunctionArgument("b", "UUID") }, ReturnType = "UUID", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-uuid-spi", "SPICombineUuid", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID", "= '123e4567-e89b-12d3-bb6d-6bb9bd380a11'::UUID" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiCombineUUId(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiCombineUUIdTestsCSharp : BaseSpiCombineUUIdTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    Guid a = reader.GetFieldValue<Guid>(reader.GetOrdinal(""UUIDCOL""));
    string aStr = a.ToString();
    string bStr = b.ToString();
    var aList = aStr.Split('-');
    var bList = bStr.Split('-');
    string newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4];
    return new Guid(newUuuidStr);
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}