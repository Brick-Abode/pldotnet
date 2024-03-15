using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Do")]
public class DoLanguageTests : PlDotNetTest
{
    private static readonly string DoBody = @"
do $$
    string arabic = ""هل تتكلم العربية؟"";
    Elog.Info($""Do you speak Arabic? => {arabic}"");

    string chinese = ""你会说中文吗？"";
    Elog.Info($""Do you speak Chinese? => {chinese}"");

    string japanese = ""あなたは日本語を話しますか？"";
    Elog.Info($""Do you speak Japanese? => {japanese}"");

    string portuguese = ""Você fala português?"";
    Elog.Info($""Do you speak Portuguese? => {portuguese}"");

    string russian = ""а ты говоришь по русски?"";
    Elog.Info($""Do you speak Russian? => {russian}"");
$$ language plcsharp;
    ";

	public DoLanguageTests()
	{
		FunctionInfo = new SqlFunctionInfo
		{
			TestType = SqlTestType.DoBlock,
		};
	}

    [Fact]
    public void TestDoLanguage()
    {
        bool doExecutedSuccessfully = ExecuteSql(DoBody);
        Assert.True(doExecutedSuccessfully);
    }
}

