
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Uuid")]
public class CombineUUIdsFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
let a = if a.HasValue then a.Value else Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"")
let b = if b.HasValue then b.Value else Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"")
let aStr = a.ToString()
let bStr = b.ToString()
let aList = aStr.Split('-')
let bList = bStr.Split('-')
let newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4]
Guid(newUuuidStr)
    ";

    public CombineUUIdsFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CombineUUIdsFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID"), new FunctionArgument("b", "UUID") },
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
            new object[] { "f#-uuid", "combineUUIDsFSharp1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID", "= 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID" },
        new object[] { "f#-uuid", "combineUUIDsFSharp2", "'123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= '123e4567-e89b-12d3-9694-12769239b763'::UUID" },
        new object[] { "f#-uuid-null", "combineUUIDsFSharp3", "NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCombineUUIdsFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
