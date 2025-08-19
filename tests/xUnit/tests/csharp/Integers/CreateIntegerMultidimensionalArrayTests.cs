using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

public abstract class BaseCreateIntegerMultidimensionalArrayTests : PlDotNetTest
{
    protected abstract string FunctionBody { get; }

    protected abstract LanguageType Language { get; }

    public BaseCreateIntegerMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo { Name = "CreateIntegerMultidimensionalArray", Arguments = new List<FunctionArgument> { }, ReturnType = "integer[]", Body = FunctionBody, Language = Language, IsStrict = true, };
    }

    public static object[][] TestCases()
    {
        return new object[][] { new object[] { "c#-int4-null-3array-arraynull", "CreateIntegerMultidimensionalArray", "", "= ARRAY[[[2047483647::integer, 304325::integer], [null::integer, null::integer]], [[706524::integer, 9652345::integer], [943::integer, 4134677::integer]]]" }, };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateIntegerMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class CreateIntegerMultidimensionalArrayTestsCSharp : BaseCreateIntegerMultidimensionalArrayTests
{
    protected override string FunctionBody => @"
int?[, ,] integer_three_dimensional = new int?[2, 2, 2] {{{2047483647, 304325}, {null, null}}, {{706524, 9652345}, {943, 4134677}}};
return integer_three_dimensional;
    ";
    protected override LanguageType Language => LanguageType.PlcSharp;
}