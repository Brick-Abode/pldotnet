
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class ReturnSmallInTArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return small_integers;
    ";

    public ReturnSmallInTArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnSmallInTArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("small_integers", "smallint[]") },
            ReturnType = "smallint[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int2-null-1array", "returnSmallIntArray1", "ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]", "= ARRAY[12345::smallint, null::smallint, 123::smallint, 4356::smallint]" },
        new object[] { "c#-int2-null-2array-arraynull", "returnSmallIntArray2", "ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]", "= ARRAY[[null::smallint, null::smallint], [12345::smallint, 654::smallint]]" },
        new object[] { "c#-int2-null-3array-arraynull", "returnSmallIntArray3", "ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]]", "= ARRAY[[[null::smallint, null::smallint], [null::smallint, null::smallint]], [[186::smallint, 23823::smallint], [9521::smallint, 934::smallint]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnSmallInTArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
