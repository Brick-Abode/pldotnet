using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseTableTypeTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseTableTypeTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "TableTypeTest",
            ReturnType = "TABLE(id integer, name text)",
            Body = FunctionBody,
            Language = Language,
            IsStrict = false,
        };
    }

    protected void SetupTest(string cteStatement)
    {
        this.cteStatement = cteStatement;
        FunctionInfo!.CteStatement = cteStatement;
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[]
        {
            "c#-table-function",
            "type-integrity",
            @"WITH data AS (SELECT * FROM TableTypeTest())",
            "BOOL_AND(pg_typeof(id) = 'integer'::regtype AND pg_typeof(name) = 'text'::regtype)"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestTableType(
        string featureName,
        string testName,
        string cteStatement,
        string customAssertion,
        string querySuffix = null!
    )
    {
        SetupTest(cteStatement);
        RunTestWithSuffix(featureName, testName, this.cteStatement, customAssertion, querySuffix);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Table")]
public class TableTypeTestsCsharp : BaseTableTypeTests
{
    protected override string FunctionBody =>
        @"
yield return (1, ""Alice"");
yield return (2, ""Bob"");
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
