
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Geometric")]
public class UpdateArrayPathIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
values_array.SetValue(desired, arrayInteger);
return values_array;
    ";

    public UpdateArrayPathIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayPathIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("values_array", "PATH[]"), new FunctionArgument("desired", "PATH"), new FunctionArgument("index", "integer[]") },
            ReturnType = "PATH[]",
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
            new object[] { "c#-path-null-1array", "updateArrayPathIndex1", "ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, ARRAY[2]", "= CAST(ARRAY['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH] AS TEXT)" },
            new object[] { "c#-path-null-2array-arraynull", "updateArrayPathIndex2", "ARRAY[[null::PATH, null::PATH], [null::path, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]], '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, ARRAY[1,0]", "= CAST(ARRAY[[null::PATH, null::PATH], ['((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH, '((1.5,2.75),(3.0,4.75),(5.0,5.0))'::PATH]] AS TEXT)" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayPathIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
