
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "BooL")]
public class UpdateArrayBooleanIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
booleans.SetValue(desired, arrayInteger);
return booleans;
    ";

    public UpdateArrayBooleanIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayBooleanIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("booleans", "boolean[]"), new FunctionArgument("desired", "boolean"), new FunctionArgument("index", "integer[]") },
            ReturnType = "boolean[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-bool-1array", "updateArrayBooleanIndex1", "ARRAY[true, false, true], true, ARRAY[1]", "= ARRAY[true, true, true]" },
            new object[] { "c#-bool-2array", "updateArrayBooleanIndex2", "ARRAY[[true, false], [true, false]], false, ARRAY[1, 0]", "= ARRAY[[true, false], [false, false]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayBooleanIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
