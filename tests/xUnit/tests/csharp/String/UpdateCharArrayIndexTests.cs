
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class UpdateCharArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateCharArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateCharArrayIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "BPCHAR[]"), new FunctionArgument("desired", "BPCHAR"), new FunctionArgument("index", "integer[]") },
            ReturnType = "BPCHAR[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bpchar-null-1array", "updateCharArrayIndex1", "ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]", "= ARRAY['hello'::BPCHAR, 'hi'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR]" },
        new object[] { "c#-bpchar-null-2array-arraynull", "updateCharArrayIndex2", "ARRAY[[null::BPCHAR, null::BPCHAR], [null::BPCHAR, 'bye'::BPCHAR]], 'goodbye'::BPCHAR, ARRAY[1,0]", "= ARRAY[[null::BPCHAR, null::BPCHAR], ['goodbye'::BPCHAR, 'bye'::BPCHAR]]" },
        new object[] { "c#-bpchar-null-1array", "updateCharArrayIndex3", "ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, null::BPCHAR, 'bye'::BPCHAR], 'goodbye'::BPCHAR, ARRAY[2]", "= ARRAY['goodbye'::BPCHAR, 'bye'::BPCHAR, 'goodbye'::BPCHAR, 'bye'::BPCHAR]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateCharArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
