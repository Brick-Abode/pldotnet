using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateIntervalMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateIntervalMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateIntervalMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "INTERVAL[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-interval-3array", "CreateIntervalMultidimensionalArray", "", "= ARRAY[[[INTERVAL '10 months 33 days 2 hours 30 minutes', INTERVAL '10 months 33 days 2 hours 30 minutes'], [null::interval, null::interval]], [[INTERVAL '10 months 33 days 2 hours 30 minutes', null::interval], [INTERVAL '10 months 33 days 2 hours 30 minutes', INTERVAL '10 months 33 days 2 hours 30 minutes']]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateIntervalMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateIntervalMultidimensionalArrayTestsCSharp : BaseCreateIntervalMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
int months = 10;
int days = 33;
long time = 9000000000;
NpgsqlInterval objects_value = new NpgsqlInterval(months, days, time);
NpgsqlInterval?[, ,] three_dimensional_array = new NpgsqlInterval?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}