
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "String")]
public class CreateTextMultidimensionalArrayTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
string?[, ,] text_three_dimensional = new string?[2, 2, 2] {{{""text 1"", ""text 2""}, {null, null}}, {{""text 3"", null}, {""text 4"", ""text5""}}};
return text_three_dimensional;
    ";

    public CreateTextMultidimensionalArrayTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "CreateTextMultidimensionalArray",
            Arguments = new List<FunctionArgument> {  },
            ReturnType = "text[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-text-null-3array-arraynull", "CreateTextMultidimensionalArray", "", "= ARRAY[[['text 1'::text, 'text 2'::text], [null::text, null::text]], [['text 3'::text, null::text], ['text 4'::text, 'text5'::text]]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestCreateTextMultidimensionalArray(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
