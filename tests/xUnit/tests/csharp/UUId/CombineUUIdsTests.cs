
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "UUId")]
public class CombineUUIdsTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
if (a == null)
        a = new Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"");

    if (b == null)
        b = new Guid(""a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"");
    
    string aStr = a.ToString();
    string bStr = b.ToString();
    var aList = aStr.Split('-');
    var bList = bStr.Split('-');
    string newUuuidStr = aList[0] + aList[1] + aList[2] + bList[3] + bList[4];
    return new Guid(newUuuidStr);
    ";

    public CombineUUIdsTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CombineUUIds",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "UUID"), new FunctionArgument("b", "UUID") },
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
            new object[] { "c#-uuid", "combineUUIDs1", "'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'::UUID, '87e3006a-604e-11ed-9b6a-0242ac120002'::UUID", "= 'a0eebc99-9c0b-4ef8-9b6a-0242ac120002'::UUID" },
        new object[] { "c#-uuid", "combineUUIDs2", "'123e4567-e89b-12d3-a456-426614174000'::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= '123e4567-e89b-12d3-9694-12769239b763'::UUID" },
        new object[] { "c#-uuid-null", "combineUUIDs3", "NULL::UUID, '024be913-3bf8-4499-9694-12769239b763'::UUID", "= 'a0eebc99-9c0b-4ef8-9694-12769239b763'::UUID" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCombineUUIds(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
