using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public abstract class BaseDoMessageFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseDoMessageFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { TestType = SqlTestType.DoBlock, };
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "Do")]
public class DoMessageFsharpTestsFSharp : BaseDoMessageFsharpTests
{
    protected override string FunctionBody => @"
do $$
let message = ""PL.NET IS THE BEST PROCEDURE LANGUAGE!""
Elog.Info(message);
$$ language plfsharp;
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}