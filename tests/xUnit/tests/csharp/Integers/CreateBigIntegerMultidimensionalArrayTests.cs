
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class CreateBigIntegerMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
long?[, ,] big_integer_three_dimensional = new long?[2, 2, 2] {{{92232036854775707, 2337203684775707}, {null, null}}, {{706524, 756452434247987}, {943, 4134677}}};
return big_integer_three_dimensional;
    ";

    public CreateBigIntegerMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateBigIntegerMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "bigint[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int8-null-3array-arraynull", "CreateBigIntegerMultidimensionalArray", "", "= ARRAY[[[92232036854775707::bigint, 2337203684775707::bigint], [null::bigint, null::bigint]], [[706524::bigint, 756452434247987::bigint], [943::bigint, 4134677::bigint]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateBigIntegerMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
