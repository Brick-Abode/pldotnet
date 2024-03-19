
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Network")]
public class CreateMacAddress8MultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
byte[] bytes = new byte[8] {171, 1, 43, 49, 65, 250, 171, 172};
PhysicalAddress objects_value = new PhysicalAddress(bytes);
PhysicalAddress?[, ,] three_dimensional_array = new PhysicalAddress?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateMacAddress8MultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateMacAddress8MultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "MACADDR8[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-macaddr8-3array", "CreateMacAddress8MultidimensionalArray", "", "= ARRAY[[[MACADDR8 'ab-01-2b-31-41-fa-ab-ac', MACADDR8 'ab-01-2b-31-41-fa-ab-ac'], [null::MACADDR8, null::MACADDR8]], [[MACADDR8 'ab-01-2b-31-41-fa-ab-ac', null::MACADDR8], [MACADDR8 'ab-01-2b-31-41-fa-ab-ac', MACADDR8 'ab-01-2b-31-41-fa-ab-ac']]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateMacAddress8MultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
