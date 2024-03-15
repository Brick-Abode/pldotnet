using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Do")]
public class DoSum2Tests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    int c = 1450 + 275;
    Elog.Info($""c = {c}"");
$$ language plcsharp;
    ";

	public DoSum2Tests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoSum2()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

