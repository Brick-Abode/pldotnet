
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class AddDaysTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (my_timestamp == null) {
    my_timestamp = new DateTime(2022, 1, 1, 8, 30, 20, DateTimeKind.Utc);
}

return ((DateTime)my_timestamp).AddDays((double)days_to_add);
    ";

    public AddDaysTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "AddDays",
            Arguments = new List<FunctionArgument> { new FunctionArgument("my_timestamp TIMESTAMP WITH TIME", "ZONE"), new FunctionArgument("days_to_add", "INT") },
            ReturnType = "TIMESTAMP WITH TIME ZONE",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-timestamptz", "addDays1", "TIMESTAMP WITH TIME ZONE '2004-10-19 10:23:54 PM +02', 2", "= TIMESTAMP WITH TIME ZONE '2004-10-21 22:23:54 +02'" },
        new object[] { "c#-timestamptz-null", "addDays2", "NULL::TIMESTAMP WITH TIME ZONE, 2", "= TIMESTAMP WITH TIME ZONE '2022-01-03 08:30:20 +00'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestAddDays(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
