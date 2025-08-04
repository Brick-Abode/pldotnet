using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BaseDoSum2FsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseDoSum2FsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.DoBlock, };
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoSum2FsharpTestsFSharp : BaseDoSum2FsharpTests
{
    protected override string FunctionBody => @"
do $$
    let c = 1450 + 275;
    Elog.Info(""c = "" + c.ToString());
$$ language plfsharp;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}