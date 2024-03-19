
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class UpdateArrayBigIntegerIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
big_integers.SetValue(desired, arrayInteger);
return big_integers;
    ";

    public UpdateArrayBigIntegerIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayBigIntegerIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("big_integers", "bigint[]"), new FunctionArgument("desired", "bigint"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-int8-null-1array", "updateArrayBigIntegerIndex1", "ARRAY[92232036854775707::bigint, 2337203684775707::bigint, null::bigint], CAST(67337203684775707 AS BIGINT), ARRAY[1]", "= ARRAY[92232036854775707::bigint, 67337203684775707::bigint, null::bigint]" },
        new object[] { "c#-int8-null-2array", "updateArrayBigIntegerIndex2", "ARRAY[[92232036854775707::bigint, 2337203684775707::bigint], [null::bigint, 12465464::bigint]], CAST(67337203684775707 AS BIGINT), ARRAY[1, 0]", "= ARRAY[[92232036854775707::bigint, 2337203684775707::bigint], [67337203684775707::bigint, 12465464::bigint]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayBigIntegerIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
