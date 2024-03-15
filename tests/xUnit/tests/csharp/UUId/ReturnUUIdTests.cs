
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "UUId")]
public class ReturnUUIdTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
return a;
    ";

    public ReturnUUIdTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "ReturnUUId",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID") },
            ReturnType = "UUID",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-uuid", "returnUUID1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID", "= 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID" },
        new object[] { "c#-uuid", "returnUUID2", "'123e4567-e89b-12d3-a456-426614174000'::UUID", "= '123e4567-e89b-12d3-a456-426614174000'::UUID" },
        new object[] { "c#-uuid", "returnUUID3", "'87e3006a-604e-11ed-9b6a-0242ac120002'::UUID", "= '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID" },
        new object[] { "c#-uuid", "returnUUID4", "'024be913-3bf8-4499-9694-12769239b763'::UUID", "= '024be913-3bf8-4499-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestReturnUUId(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
