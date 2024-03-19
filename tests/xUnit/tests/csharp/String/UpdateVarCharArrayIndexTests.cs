
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class UpdateVarCharArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateVarCharArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateVarCharArrayIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "VARCHAR[]"), new FunctionArgument("desired", "VARCHAR"), new FunctionArgument("index", "integer[]") },
            ReturnType = "VARCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-varchar-null-1array", "updateVarcharArrayIndex1", "ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, null::VARCHAR, 'bye'::VARCHAR], 'goodbye'::VARCHAR, ARRAY[2]", "= ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, 'goodbye'::VARCHAR, 'bye'::VARCHAR]" },
        new object[] { "c#-varchar-null-2array-arraynull", "updateVarcharArrayIndex2", "ARRAY[[null::VARCHAR, null::VARCHAR], [null::VARCHAR, 'bye'::VARCHAR]], 'goodbye'::VARCHAR, ARRAY[1,0]", "= ARRAY[[null::VARCHAR, null::VARCHAR], ['goodbye'::VARCHAR, 'bye'::VARCHAR]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateVarCharArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
