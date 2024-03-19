using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Do")]
public class DoSum1Tests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    int c = 10 + 25;
    Elog.Info($""c = {c}"");
$$ language plcsharp;
    ";

	public DoSum1Tests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoSum1()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

