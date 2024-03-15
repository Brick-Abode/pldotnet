
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class ReturnDoubleArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return doubles;
    ";

    public ReturnDoubleArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnDoubleArray",
            Arguments = new List<FunctionArgument> { new FunctionArgument("doubles double", "precision[]") },
            ReturnType = "double precision[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-float8-null-1array", "returnDoubleArray1", "ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]", "= ARRAY[21.0000000000109::double precision, null::double precision, 4.521234313421::double precision, 7.412344328978::double precision]" },
        new object[] { "c#-float8-null-2array-arraynull", "returnDoubleArray2", "ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]", "= ARRAY[[null::double precision, null::double precision], [4.521234313421::double precision, 7.412344328978::double precision]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnDoubleArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
