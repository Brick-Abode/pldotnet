
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class CreateLineMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
NpgsqlLine objects_value = new NpgsqlLine(2.4, 8.2, -32.43);
NpgsqlLine?[, ,] three_dimensional_array = new NpgsqlLine?[2, 2, 2] {{{objects_value, objects_value}, {null, null}}, {{objects_value, null}, {objects_value, objects_value}}};
return three_dimensional_array;
    ";

    public CreateLineMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateLineMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "LINE[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
            CastFunctionAs = "TEXT",
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-line-3array", "CreateLineMultidimensionalArray1", "", "= CAST(ARRAY[[[LINE '{2.4,8.2,-32.43}', LINE '{2.4,8.2,-32.43}'], [null::LINE, null::LINE]], [[LINE '{2.4,8.2,-32.43}', null::LINE], [LINE '{2.4,8.2,-32.43}', LINE '{2.4,8.2,-32.43}']]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateLineMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
