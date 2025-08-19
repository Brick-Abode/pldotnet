using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BaseDoSum1FsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseDoSum1FsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.DoBlock, };
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoSum1FsharpTestsFSharp : BaseDoSum1FsharpTests
{
    protected override string FunctionBody => @"
do $$
    let c = 10 + 25
    Elog.Info(""c = "" + c.ToString());
$$ language plfsharp;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}