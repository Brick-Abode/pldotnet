
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Uuid")]
public class CreateUUIdfSharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
Guid(a)
    ";

    public CreateUUIdfSharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateUUIdfSharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "TEXT") },
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
            new object[] { "f#-uuid", "createUUIDFSharp1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::TEXT", "= 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID" },
        new object[] { "f#-uuid", "createUUIDFSharp2", "'123e4567-e89b-12d3-a456-426614174000'::TEXT", "= '123e4567-e89b-12d3-a456-426614174000'::UUID" },
        new object[] { "f#-uuid", "createUUIDFSharp3", "'87e3006a-604e-11ed-9b6a-0242ac120002'::TEXT", "= '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID" },
        new object[] { "f#-uuid", "createUUIDFSharp4", "'024be913-3bf8-4499-9694-12769239b763'::TEXT", "= '024be913-3bf8-4499-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateUUIdfSharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
