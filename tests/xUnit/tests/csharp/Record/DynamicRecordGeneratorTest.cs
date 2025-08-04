using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseDynamicRecordGeneratorTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseDynamicRecordGeneratorTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "DynamicRecordGenerator",
            Arguments = new List<FunctionArgument> { new FunctionArgument("scenario", "int4") },
            ReturnType = "record",
            Language = Language,
            IsStrict = false,
            Body = FunctionBody
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
            "c#-drec",
            "drec-comparison-1a",
            @"WITH cte AS (SELECT * FROM DynamicRecordGenerator(1) AS (a int4, b text))",
            "a = 1"
        };

        yield return new object[]
        {
            "c#-drec",
            "drec-comparison-1b",
            @"WITH cte AS (SELECT * FROM DynamicRecordGenerator(1) AS (a int4, b text))",
            "b = 'Alice'"
        };

        yield return new object[]
        {
            "c#-drec",
            "drec-comparison-2a",
            @"WITH cte AS (SELECT * FROM DynamicRecordGenerator(2) AS (a int4, b varchar))",
            "a = 2"
        };

        yield return new object[]
        {
            "c#-drec",
            "drec-comparison-2b",
            @"WITH cte AS (SELECT * FROM DynamicRecordGenerator(2) AS (a int4, b varchar))",
            "b = 'Barbara'"
        };

        yield return new object[]
        {
            "c#-drec",
            "drec-comparison-3",
            @"WITH cte AS (SELECT * FROM DynamicRecordGenerator(3) AS (a float, b float, c bool))",
            "c = true"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDynamicRecordGenerator(
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
[Trait("Category", "Record")]
public class DynamicRecordGeneratorTestsCsharp : BaseDynamicRecordGeneratorTests
{
    protected override string FunctionBody =>
        @"
switch(scenario)
{
    case 1:
        return new object[]{1, ""Alice""};
    case 2:
        var barbara = new NpgsqlParameter
                {
                    ParameterName = ""_"",
                    NpgsqlDbType = NpgsqlDbType.Varchar,
                    Value = ""Barbara""
                };
        return new object[]{2, barbara};
    case 3:
       return new object[]{1.5, 2.5, true};
    default:
        throw new SystemException($""Unrecognized scenario: {scenario}"");
}
";

    protected override LanguageType Language => LanguageType.PlcSharp;
}
