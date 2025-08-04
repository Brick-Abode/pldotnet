using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

public abstract class BaseDynamicRecordGeneratorTypesTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }
    protected abstract LanguageType Language { get; }

    protected string cteStatement = string.Empty;

    public BaseDynamicRecordGeneratorTypesTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "DynamicRecordGeneratorTypes",
            Arguments = new List<FunctionArgument> { new FunctionArgument("scenario", "INT4") },
            ReturnType = "record",
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
            "c#-drec-types",
            "drec-type-1",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(1)
        AS (a int4, b int2)
)",
            "b = 1"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-2",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(2)
        AS (a int4, b int4)
)",
            "b = 2"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-3",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(3)
        AS (a int4, b int8)
)",
            "b = 3"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-4",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(4)
        AS (a int4, b text)
)",
            "b = '4'"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-5",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(5)
        AS (a int4, b varchar)
)",
            "b = 'five'::varchar"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-6a",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(6)
        AS (a int4, b float4)
)",
            "b > 5.999"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-6b",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(6)
        AS (a int4, b float4)
)",
            "b < 6.001"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-7a",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(7)
        AS (a int4, b float8)
)",
            "b > 6.999"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "drec-type-7b",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(7)
        AS (a int4, b float8)
)",
            "b < 7.001"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "null-is-present",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(8)
        AS (a int4, b float4)
)",
            "(a IS NULL AND b IS NULL)"
        };

        yield return new object[]
        {
            "c#-drec-types",
            "null-is-not-present",
            @"WITH cte AS (
    SELECT * FROM DynamicRecordGeneratorTypes(6)
        AS (a int4, b float4)
)",
            "(a IS NOT NULL AND b IS NOT NULL)"
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestDynamicRecordGeneratorTypesTests(
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
public class DynamicRecordGeneratorTypesTestsCsharp : BaseDynamicRecordGeneratorTypesTests
{
    protected override string FunctionBody =>
        @"
switch(scenario)
    {
        case 1: // short
            return new object[]{101, (short)1};
        case 2: // int
            return new object[]{102, (int)2};
        case 3: // long
            return new object[]{103, (long)3};
        case 4: // string
            return new object[]{104, ""4""};
        case 5: // varchar
            var five = new NpgsqlParameter
                    {
                        ParameterName = ""_"",
                        NpgsqlDbType = NpgsqlDbType.Varchar,
                        Value = ""five""
                    };
            return new object[]{105, five};
        case 6: // float
            return new object[]{101, (float)6.0};
        case 7: // double
            return new object[]{101, (double)7.0};
        case 8:
            var nullInt = new NpgsqlParameter
            {
                ParameterName = ""_"",
                NpgsqlDbType = NpgsqlDbType.Integer
            };
            var nullFloat = new NpgsqlParameter
            {
                ParameterName = ""_"",
                NpgsqlDbType = NpgsqlDbType.Real
            };
            return new object[]{nullInt, nullFloat};
        default:
            throw new SystemException($""Unrecognized scenario: {scenario}"");
    }
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}
