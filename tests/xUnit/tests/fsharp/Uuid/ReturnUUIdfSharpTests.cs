
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Uuid")]
public class ReturnUUIdfSharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
a
    ";

    public ReturnUUIdfSharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnUUIdfSharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID") },
            ReturnType = "UUID",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-uuid", "returnUUIDFSharp1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID", "= 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID" },
        new object[] { "f#-uuid", "returnUUIDFSharp2", "'123e4567-e89b-12d3-a456-426614174000'::UUID", "= '123e4567-e89b-12d3-a456-426614174000'::UUID" },
        new object[] { "f#-uuid", "returnUUIDFSharp3", "'87e3006a-604e-11ed-9b6a-0242ac120002'::UUID", "= '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID" },
        new object[] { "f#-uuid", "returnUUIDFSharp4", "'024be913-3bf8-4499-9694-12769239b763'::UUID", "= '024be913-3bf8-4499-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnUUIdfSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
