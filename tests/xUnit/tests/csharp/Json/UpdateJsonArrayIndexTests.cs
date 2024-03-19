
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Json")]
public class UpdateJsonArrayIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateJsonArrayIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateJsonArrayIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "JSON[]"), new FunctionArgument("desired", "JSON"), new FunctionArgument("index", "integer[]") },
            ReturnType = "JSON[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp,
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
new object[]
{
    "c#-json-null-1array",
    "updateJsonArrayIndex1",
    "ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, ARRAY[2]",
    "= ARRAY['{\"age\": 20, \"name\": \"Mikael\"}'::JSON, '{\"age\": 25, \"name\": \"Rosicley\"}'::JSON, '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]::TEXT"
},
        new object[]
        {
            "c#-json-null-2array-arraynull",
            "updateJsonArrayIndex2",
            "ARRAY[[null::JSON, null::JSON], [null::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]], '{\"age\": 40, \"name\": \"John Doe\"}'::JSON, ARRAY[1,0]",
            "= ARRAY[[null::JSON, null::JSON], ['{\"age\": 40, \"name\": \"John Doe\"}'::JSON, '{\"age\": 30, \"name\": \"Todd\"}'::JSON]]::TEXT"
        },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateJsonArrayIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
