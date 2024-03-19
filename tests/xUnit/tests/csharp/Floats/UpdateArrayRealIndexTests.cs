
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Floats")]
public class UpdateArrayRealIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
floats.SetValue(desired, arrayInteger);
return floats;
    ";

    public UpdateArrayRealIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayRealIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("floats", "real[]"), new FunctionArgument("desired", "real"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-float4-null-1array", "updateArrayRealIndex1", "ARRAY[4.55555::real, 10.11324::real, null::real], 9.83212, ARRAY[1]", "= ARRAY[4.55555::real, 9.83212::real, null::real]" },
        new object[] { "c#-float4-null-2array", "updateArrayRealIndex2", "ARRAY[[4.55555::real, 10.11324::real], [null::real, 16.12464::real]], 9.83212, ARRAY[1, 0]", "= ARRAY[[4.55555::real, 10.11324::real], [9.83212::real, 16.12464::real]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayRealIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
