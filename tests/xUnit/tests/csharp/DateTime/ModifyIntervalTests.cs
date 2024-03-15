
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class ModifyIntervalTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_interval == null) {
    orig_interval = new NpgsqlInterval(4, 25, 9000000000);
}

NpgsqlInterval new_interval = new NpgsqlInterval(((NpgsqlInterval)orig_interval).Months + (int)months_to_add, ((NpgsqlInterval)orig_interval).Days + (int)days_to_add, ((NpgsqlInterval)orig_interval).Time);
return new_interval;
    ";

    public ModifyIntervalTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ModifyInterval",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_interval", "INTERVAL"), new FunctionArgument("days_to_add", "INT"), new FunctionArgument("months_to_add", "INT") },
            ReturnType = "INTERVAL",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-interval", "modifyInterval1", "INTERVAL '4 hours 5 minutes 6 seconds', 15, 20", "= INTERVAL '1 YEAR 8 MONTHS 15 DAYS 4 HOURS 5 MINUTES 6 SECONDS'" },
        new object[] { "c#-interval-null", "modifyInterval2", "NULL::INTERVAL, 15, 20", "= INTERVAL '2 YEAR 40 DAYS 2 HOURS 30 MINUTES'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyInterval(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
