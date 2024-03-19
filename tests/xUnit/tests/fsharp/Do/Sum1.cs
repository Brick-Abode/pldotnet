using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoSum1FsharpTests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    let c = 10 + 25
    Elog.Info(""c = "" + c.ToString());
$$ language plfsharp;
    ";

	public DoSum1FsharpTests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoSum1Fsharp()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

