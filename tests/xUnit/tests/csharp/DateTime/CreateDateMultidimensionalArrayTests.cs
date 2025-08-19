using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateDateMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateDateMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateDateMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "DATE[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-date-null-3array-arraynull", "CreateDateMultidimensionalArray", "", "= ARRAY[[[DATE 'Dec-25-2022'::date, DATE 'Dec-25-2022'::date], [null::date, null::date]], [[DATE 'Dec-25-2022'::date, null::date], [DATE 'Dec-25-2022'::date, DATE 'Dec-25-2022'::date]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDateMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class CreateDateMultidimensionalArrayTestsCSharp : BaseCreateDateMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
int day = 25;
int month = 12;
int year = 2022;
DateOnly arrayDate = new DateOnly(year,month,day);
DateOnly?[, ,] three_dimensional_array = new DateOnly?[2, 2, 2] {{{arrayDate, arrayDate}, {null, null}}, {{arrayDate, null}, {arrayDate, arrayDate}}};
return three_dimensional_array;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}