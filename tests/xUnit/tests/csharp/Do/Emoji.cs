using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Do")]
public class DoEmojiTests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    string emoji = ""🐂"";
	Elog.Info($""The emoji \""{emoji}\"" has lenght {emoji.Length}."");

    emoji = ""\ud83e\udd70"";
	Elog.Info($""The emoji \""{emoji}\"" has lenght {emoji.Length}."");
$$ language plcsharp;
    ";

	public DoEmojiTests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoEmoji()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

