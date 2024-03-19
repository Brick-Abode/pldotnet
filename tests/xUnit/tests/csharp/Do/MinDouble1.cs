using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Do")]
public class DoMinDouble1Tests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    double[] doublevalues = {2.25698, 2.85956, 2.85456, 0.00128, 0.00127, 2.36875};
    double min = double.MaxValue;
    for(int i = 0; i < doublevalues.Length; i++)
    {
        double value = (double)doublevalues.GetValue(i);
        min = min < value ? min : value;
    }
    Elog.Info($""Minimum value = {min}"");
$$ language plcsharp;
    ";

	public DoMinDouble1Tests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoMinDouble1()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

