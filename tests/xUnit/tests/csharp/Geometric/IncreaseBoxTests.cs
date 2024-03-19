
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class IncreaseBoxTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (orig_value == null)
    orig_value = new NpgsqlBox(new NpgsqlPoint(0, 0), new NpgsqlPoint(100, 100));

NpgsqlBox new_value = new NpgsqlBox(new NpgsqlPoint(((NpgsqlBox)orig_value).UpperRight.X + 1, ((NpgsqlBox)orig_value).UpperRight.Y + 1), new NpgsqlPoint(((NpgsqlBox)orig_value).LowerLeft.X + 1, ((NpgsqlBox)orig_value).LowerLeft.Y + 1));

return new_value;
    ";

    public IncreaseBoxTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "IncreaseBox",
            Arguments = new List<FunctionArgument> { new FunctionArgument("orig_value", "BOX") },
            ReturnType = "BOX",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-box-null", "increaseBox1", "NULL::BOX", "= BOX(POINT(101,101),POINT(1,1))" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestIncreaseBox(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
