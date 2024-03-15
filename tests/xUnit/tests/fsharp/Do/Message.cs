using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoMessageFsharpTests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
let message = ""PL.NET IS THE BEST PROCEDURE LANGUAGE!""
Elog.Info(message);
$$ language plfsharp;
    ";

	public DoMessageFsharpTests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoMessageFsharp()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

