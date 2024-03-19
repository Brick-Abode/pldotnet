
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "String")]
public class UpdateVarCharArrayIndexFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let arrayInteger: int[] = index.Cast<int>().ToArray()
values_array.SetValue(desired, arrayInteger)
values_array
    ";

    public UpdateVarCharArrayIndexFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateVarCharArrayIndexFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "VARCHAR[]"), new FunctionArgument("desired", "VARCHAR"), new FunctionArgument("index", "integer[]") },
            ReturnType = "VARCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-varchar-null-1array", "updateVarcharArrayIndexFSharp1", "ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, null::VARCHAR, 'bye'::VARCHAR], 'goodbye'::VARCHAR, ARRAY[2]", "= ARRAY['hello'::VARCHAR, 'hi'::VARCHAR, 'goodbye'::VARCHAR, 'bye'::VARCHAR]" },
        new object[] { "f#-varchar-null-2array-arraynull", "updateVarcharArrayIndexFSharp2", "ARRAY[[null::VARCHAR, null::VARCHAR], [null::VARCHAR, 'bye'::VARCHAR]], 'goodbye'::VARCHAR, ARRAY[1,0]", "= ARRAY[[null::VARCHAR, null::VARCHAR], ['goodbye'::VARCHAR, 'bye'::VARCHAR]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateVarCharArrayIndexFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
