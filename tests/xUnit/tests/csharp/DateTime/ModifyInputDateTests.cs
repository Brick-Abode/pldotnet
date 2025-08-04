using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseModifyInputDateTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseModifyInputDateTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "ModifyInputDate", Arguments = new List<FunctionArgument> { new FunctionArgument("orig_date", "DATE") }, ReturnType = "DATE", Body = FunctionBody, Language = Language, IsStrict = false, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-date", "modifyInputDate1", "DATE 'Oct-14-2022'", "= DATE 'Nov-20-2025'" }, new object[] { "c#-date-null", "modifyInputDate2", "NULL::DATE", "= DATE 'Feb-07-2025'" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestModifyInputDate(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "DateTime")]
public class ModifyInputDateTestsCSharp : BaseModifyInputDateTests
{
    protected override string FunctionBody => @"
if (orig_date == null) {
    orig_date = new DateOnly(2022, 1, 1);
}

int day = ((DateOnly)orig_date).Day;
int month = ((DateOnly)orig_date).Month;
int year = ((DateOnly)orig_date).Year;
DateOnly new_date = new DateOnly(year+3,month+1,day+6);
return new_date;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}