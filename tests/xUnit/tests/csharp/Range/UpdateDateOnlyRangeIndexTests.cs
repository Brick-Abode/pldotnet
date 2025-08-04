using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateDateOnlyRangeIndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateDateOnlyRangeIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateDateOnlyRangeIndex", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "DATERANGE[]"), new FunctionArgument("desired", "DATERANGE"), new FunctionArgument("index", "integer[]") }, ReturnType = "DATERANGE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-daterange-null-1array", "updateDateonlyRangeIndex1", "ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, null::DATERANGE, '[,)'::DATERANGE], '[2021-05-25,)'::DATERANGE, ARRAY[2]", "= ARRAY['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE, '[2021-05-25,)'::DATERANGE, '[,)'::DATERANGE]" }, new object[] { "c#-daterange-null-2array", "updateDateonlyRangeIndex2", "ARRAY[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE, ARRAY[1, 0]", "= ARRAY[['[2021-01-01, 2021-01-01)'::DATERANGE, '(, 2021-04-01)'::DATERANGE], ['[2021-05-25,)'::DATERANGE, '[,)'::DATERANGE]]" }, new object[] { "c#-daterange-null-2array-arraynull", "updateDateonlyRangeIndex3", "ARRAY[[null::DATERANGE, null::DATERANGE], [null::DATERANGE, '[,)'::DATERANGE]], '[2021-05-25,)'::DATERANGE, ARRAY[1, 0]", "= ARRAY[[null::DATERANGE, null::DATERANGE], ['[2021-05-25,)'::DATERANGE, '[,)'::DATERANGE]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateDateOnlyRangeIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Range")]
public class UpdateDateOnlyRangeIndexTestsCSharp : BaseUpdateDateOnlyRangeIndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}