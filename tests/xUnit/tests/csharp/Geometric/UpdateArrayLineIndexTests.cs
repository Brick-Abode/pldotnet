
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayLineIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayLineIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayLineIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "LINE[]"), new FunctionArgument("desired", "LINE"), new FunctionArgument("index", "integer[]") },
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
            new object[] { "c#-line-1array", "updateArrayLineIndex1", "ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', null::LINE, LINE '{-1.5,2.75,-3.25}'], LINE '{-1.5,2.75,-3.25}', ARRAY[2]", "= CAST(ARRAY[LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}'] AS TEXT)" },
            new object[] { "c#-line-null-2array-arraynull", "updateArrayLineIndex2", "ARRAY[[null::LINE, null::LINE], [null::LINE, LINE '{-1.5,2.75,-3.25}']], LINE '{-1.5,2.75,-3.25}', ARRAY[1,0]", "= CAST(ARRAY[[null::LINE, null::LINE], [LINE '{-1.5,2.75,-3.25}', LINE '{-1.5,2.75,-3.25}']] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayLineIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
