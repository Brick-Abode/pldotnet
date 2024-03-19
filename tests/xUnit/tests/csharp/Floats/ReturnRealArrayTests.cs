
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnRealArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return floats;
    ";

    public ReturnRealArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnRealArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("floats", "real[]") },
            ReturnType = "real[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float4-null-1array", "returnRealArray1", "ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real]", "= ARRAY[1.50055::real, null::real, 4.52123::real, 7.41234::real]" },
        new object[] { "c#-float4-null-2array-arraynull", "returnRealArray2", "ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]]", "= ARRAY[[null::real, null::real], [4.52123::real, 7.41234::real]]" },
        new object[] { "c#-float4-null-3array-arraynull", "returnRealArray3", "ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]]", "= ARRAY[[[null::real, null::real], [null::real, null::real]], [[7.50055::real, 8.30300::real], [9.52123::real, 11.41234::real]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnRealArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
