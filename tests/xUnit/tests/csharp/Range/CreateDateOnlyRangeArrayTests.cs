using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateDateOnlyRangeArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateDateOnlyRangeArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateDateOnlyRangeArray", Arguments = new List<FunctionArgument> { }, ReturnType = "DATERANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-daterange-null-3array-arraynull", "CreateDateonlyRangeArray1", "", "= ARRAY[[['[2022-04-14, 2022-04-15)'::DATERANGE,'[2022-04-14, 2022-04-15)'::DATERANGE], [null::DATERANGE, null::DATERANGE]], [['[2022-04-14, 2022-04-15)'::DATERANGE, null::DATERANGE], ['[2022-04-14, 2022-04-15)'::DATERANGE, '[2022-04-14, 2022-04-15)'::DATERANGE]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateOnlyRangeArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class CreateDateOnlyRangeArrayTestsCSharp : BaseCreateDateOnlyRangeArrayTests
{
    protected override string FunctionBody => @"
NpgsqlRange<DateOnly> objects_value = new NpgsqlRange<DateOnly>(new DateOnly(2022, 4, 14), true, false, new DateOnly(2022, 4, 15), false, false);
NpgsqlRange<DateOnly>?[, ,] three_dimensional_array = new NpgsqlRange<DateOnly>?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}