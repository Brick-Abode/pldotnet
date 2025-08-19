using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayTimeTzIndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayTimeTzIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayTimeTzIndex", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "TIMETZ[]"), new FunctionArgument("desired", "TIMETZ"), new FunctionArgument("index", "integer[]") }, ReturnType = "TIMETZ[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-timetz-1array", "updateArrayTimetzIndex1", "ARRAY[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00', null::timetz, TIMETZ '22:30-03:00'], TIMETZ '02:30-05:00', ARRAY[2]", "= ARRAY[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00', TIMETZ '02:30-05:00', TIMETZ '22:30-03:00']" }, new object[] { "c#-timetz-2array", "updateArrayTimetzIndex2", "ARRAY[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00', ARRAY[1, 0]", "= ARRAY[[TIMETZ '05:30-03:00', TIMETZ '06:30-03:00'], [TIMETZ '02:30-05:00', TIMETZ '22:30-03:00']]" }, new object[] { "c#-timetz-null-2array-arraynull", "updateArrayTimetzIndex3", "ARRAY[[null::TIMETZ, null::TIMETZ], [null::timetz, TIMETZ '22:30-03:00']], TIMETZ '02:30-05:00', ARRAY[1, 0]", "= ARRAY[[null::TIMETZ, null::TIMETZ], [TIMETZ '02:30-05:00', TIMETZ '22:30-03:00']]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayTimeTzIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class UpdateArrayTimeTzIndexTestsCSharp : BaseUpdateArrayTimeTzIndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}