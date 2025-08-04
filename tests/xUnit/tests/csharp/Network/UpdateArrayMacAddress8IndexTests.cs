using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseUpdateArrayMacAddress8IndexTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseUpdateArrayMacAddress8IndexTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "UpdateArrayMacAddress8Index", Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "MACADDR8[]"), new FunctionArgument("desired", "MACADDR8"), new FunctionArgument("index", "integer[]") }, ReturnType = "MACADDR8[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-macaddr8-1array", "updateArrayMacAddress8Index1", "ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac'], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[2]", "= ARRAY[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac', MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']" }, new object[] { "c#-macaddr8-2array", "updateArrayMacAddress8Index2", "ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[1, 0]", "= ARRAY[[MACADDR8 '08-00-2b-01-02-03-ab-ac', MACADDR8 '09-00-2b-01-02-03-ab-ac'], [MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" }, new object[] { "c#-macaddr8-null-2array-arraynull", "updateArrayMacAddress8Index3", "ARRAY[[null::MACADDR8, null::MACADDR8], [null::macaddr8, MACADDR8 'a8-00-2b-01-02-03-ab-ac']], MACADDR8 'd1-00-2b-01-02-03-ab-ac', ARRAY[1, 0]", "= ARRAY[[null::MACADDR8, null::MACADDR8], [MACADDR8 'd1-00-2b-01-02-03-ab-ac', MACADDR8 'a8-00-2b-01-02-03-ab-ac']]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayMacAddress8Index(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class UpdateArrayMacAddress8IndexTestsCSharp : BaseUpdateArrayMacAddress8IndexTests
{
    protected override string FunctionBody => @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}