using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateDoubleMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateDoubleMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateDoubleMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "double precision[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-float8-null-3array-arraynull", "CreateDoubleMultidimensionalArray", "", "= ARRAY[[[1.243235421::double precision, 3.423454214::double precision], [null::double precision, null::double precision]], [[9.3242542134::double precision, 8.1113476543::double precision], [10.321451237::double precision, 16.142541316::double precision]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateDoubleMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class CreateDoubleMultidimensionalArrayTestsCSharp : BaseCreateDoubleMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
double?[, ,] double_three_dimensional = new double?[2, 2, 2] {{{1.243235421, 3.423454214}, {null, null}}, {{9.3242542134, 8.1113476543}, {10.321451237, 16.142541316}}};
return double_three_dimensional;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}