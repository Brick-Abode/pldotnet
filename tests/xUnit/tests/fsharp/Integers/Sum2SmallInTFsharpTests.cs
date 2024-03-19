
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Xunit;
using System.Linq;

[Trait("Language", "FSharp")]
[Trait("Category", "Integers")]
public class Sum2SmallInTFsharpTests : PlDotNetTest
{
    private static readonly string FunctionBody = @"
match (a.HasValue, b.HasValue) with
| (false, false) -> System.Nullable(int16 0)
| (true, false) -> Nullable(a.Value)
| (false, true) -> Nullable(b.Value)
| (true, true) -> Nullable (a.Value+b.Value)
    ";

    public Sum2SmallInTFsharpTests()
    {
        FunctionInfo = new SqlFunctionInfo
        {
            Name = "Sum2SmallInTFsharp",
            Arguments = new List<FunctionArgument> { new FunctionArgument("a", "int2"), new FunctionArgument("b", "int2") },
            ReturnType = "int2",
            Body = FunctionBody,
            Language = LanguageType.PlfSharp, 
            IsStrict = false,
        };
    }

    public static object[][] TestCases()
    {
        return new object[][]
        {
            new object[] { "f#-int2", "sum2SmallIntFSharp1", "CAST(32760 AS int2), CAST(7 AS int2)", "= SMALLINT '32767'" },
        new object[] { "f#-int2-null", "sum2SmallIntFSharp2", "NULL::int2, CAST(7 AS int2)", "= SMALLINT '7'" },
        new object[] { "f#-int2-null", "sum2SmallIntFSharp3", "CAST(32760 AS int2), NULL::int2", "= SMALLINT '32760'" },
        new object[] { "f#-int2-null", "sum2SmallIntFSharp4", "NULL::int2, NULL::int2", "= SMALLINT '0'" },
        };
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TestSum2SmallInTFsharp(string featureName, string testName, string input, string expectedResult)
    {
        RunGenericTest(featureName, testName, input, expectedResult);
    }
}
