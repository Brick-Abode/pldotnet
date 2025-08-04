using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyIntervalFsharpTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyIntervalFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyIntervalFsharp", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_interval", "INTERVAL"), new FunctionArgument("days_to_add", "INT"), new FunctionArgument("months_to_add", "INT") }, ReturnType = "INTERVAL", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "f#-interval", "modifyIntervalFSharp1", "INTERVAL '4 hours 5 minutes 6 seconds', 15, 20", "= INTERVAL '1 YEAR 8 MONTHS 15 DAYS 4 HOURS 5 MINUTES 6 SECONDS'" }, new object[] { "f#-interval-null", "modifyIntervalFSharp2", "NULL::INTERVAL, 15, 20", "= INTERVAL '2 YEAR 40 DAYS 15 MINUTES'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyIntervalFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "FSharp")]
[Trait("Category", "DateTime")]
public class ModifyIntervalFsharpTestsFSharp : BaseModifyIntervalFsharpTests
{
    protected override string FunctionBody => @"
let orig_interval = if orig_interval.HasValue then orig_interval.Value else NpgsqlInterval(4, 25, 900000000)
NpgsqlInterval(orig_interval.Months + months_to_add.Value, orig_interval.Days + days_to_add.Value, orig_interval.Time)
    ";
    protected override LanguageType Language => LanguageType.PlfSharp;
}