
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "CSharp")]
[Trait("Category", "Integers")]
public class UpdateArrayIntegerIndexTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
int[] arrayInteger = index.Cast<int>().ToArray();
integers.SetValue(desired, arrayInteger);
return integers;
    ";

    public UpdateArrayIntegerIndexTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "UpdateArrayIntegerIndex",
            Arguments = new List<FunctionArgument> { new FunctionArgument("integers", "integer[]"), new FunctionArgument("desired", "integer"), new FunctionArgument("index", "integer[]") },
            ReturnType = "integer[]",
            Body = FunctionBody,
            Language = LanguageType.PlcSharp, 
            IsStrict = true,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "c#-int4-null-1array", "updateArrayIntegerIndex1", "ARRAY[2047483647::integer, 304325::integer, null::integer], 65464532, ARRAY[1]", "= ARRAY[2047483647::integer, 65464532::integer, null::integer]" },
        new object[] { "c#-int4-null-2array", "updateArrayIntegerIndex2", "ARRAY[[2047483647::integer, 304325::integer], [null::integer, 12465464::integer]], 65464532, ARRAY[1, 0]", "= ARRAY[[2047483647::integer, 304325::integer], [65464532::integer, 12465464::integer]]" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestUpdateArrayIntegerIndex(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
