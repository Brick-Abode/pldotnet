using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoSum2FsharpTests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    let c = 1450 + 275;
    Elog.Info(""c = "" + c.ToString());
$$ language plfsharp;
    ";

	public DoSum2FsharpTests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoSum2Fsharp()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

