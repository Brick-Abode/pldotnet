
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class ReturnBigIntegerArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return big_integers;
    ";

    public ReturnBigIntegerArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnBigIntegerArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("big_integers", "bigint[]") },
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
            new object[] { "c#-int8-null-1array", "returnBigIntegerArray1", "ARRAY[9223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 706524::bigint]", "= ARRAY[9223372036854775707::bigint, null::bigint, 23372036854775707::bigint, 706524::bigint]" },
        new object[] { "c#-int8-null-2array-arraynull", "returnBigIntegerArray2", "ARRAY[[null::bigint, null::bigint], [9223372036854775707::bigint, 23372036854775707::bigint]]", "= ARRAY[[null::bigint, null::bigint], [9223372036854775707::bigint, 23372036854775707::bigint]]" },
        new object[] { "c#-int8-null-3array-arraynull", "returnBigIntegerArray3", "ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[9223372036854775707::bigint, 23372036854775707::bigint], [706524::bigint, 7563452434247987::bigint]]]", "= ARRAY[[[null::bigint, null::bigint], [null::bigint, null::bigint]], [[9223372036854775707::bigint, 23372036854775707::bigint], [706524::bigint, 7563452434247987::bigint]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnBigIntegerArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
