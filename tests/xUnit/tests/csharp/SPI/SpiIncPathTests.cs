using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseSpiIncPathTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseSpiIncPathTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "SpiIncPath", Arguments = new List<FunctionArgument> { new FunctionArgument("a", "POINT") }, ReturnType = "PATH", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-path-spi", "SPIIncPath", "'(3.1415,6.2830)'::POINT", "= '( (1.0,1.0), (2.0,1.0), (2.0,2.0), (2.0,1.0), (3.1415,6.2830) )'::PATH" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSpiIncPath(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "SPI")]
public class SpiIncPathTestsCSharp : BaseSpiIncPathTests
{
    protected override string FunctionBody => @"
var dataSource = NpgsqlMultiHostDataSource.Create();
    var cmd = dataSource.CreateCommand($""SELECT * FROM SPITEST"");
    var reader = cmd.ExecuteReader(CommandBehavior.Default);
    reader.Read();
    NpgsqlPath b = reader.GetFieldValue<NpgsqlPath>(reader.GetOrdinal(""PATHCOL""));
    int npts = b.Count;
    NpgsqlPath new_path = new NpgsqlPath(npts+1);
    for(int i = 0; i < npts; i++)
    {
        new_path.Add(b[i]);
    }
    new_path.Add(((NpgsqlPoint)a));
    return new_path;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}